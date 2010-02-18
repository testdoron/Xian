﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

#if	UNIT_TESTS
#pragma warning disable 1591,0419,1574,1587

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Volume.Mpr.Utilities;
using NUnit.Framework;

namespace ClearCanvas.ImageViewer.Volume.Mpr.Tests
{
	[TestFixture]
	public class SlicerFidelityTests
	{
		public SlicerFidelityTests() {}

		[TestFixtureSetUp]
		public void Initialize()
		{
			Platform.SetExtensionFactory(new NullExtensionFactory());
		}

		[Test]
		public void TestShells()
		{
			IVolumeSlicerParams[] slicerParams = new IVolumeSlicerParams[] {new VolumeSlicerParams(32, -62, 69)};
			TestVolume(true, VolumeFunction.Shells, slicerParams, "");
			TestVolume(false, VolumeFunction.Shells, slicerParams, "");
		}

		[Test]
		public void TestStripes()
		{
			IVolumeSlicerParams[] slicerParams = new IVolumeSlicerParams[] {new VolumeSlicerParams(32, -62, 69)};
			TestVolume(true, VolumeFunction.Stripes, slicerParams, "", ImageKernelFunction3X3, VolumeKernelFunction3X3X3);
		}

		private static void TestVolume(bool signed, VolumeFunction f, IEnumerable<IVolumeSlicerParams> slicerParams, string testName)
		{
			TestVolume(signed, f, slicerParams, testName, (pd, x, y) => pd.GetPixel(x, y), (fx, x, y, z) => fx.Evaluate(x, y, z));
		}

		private static void TestVolume(bool signed, VolumeFunction f, IEnumerable<IVolumeSlicerParams> slicerParams, string testName, ImageKernelFunction imageKernel, VolumeKernelFunction volumeKernel)
		{
			const int FULL_SCALE = 65535;
			VolumeFunction normalizedFunction = f.Normalize(100);

			using (Volume volume = normalizedFunction.CreateVolume(100, signed))
			{
				float offset = signed ? -32768 : 0;
				foreach (IVolumeSlicerParams slicing in slicerParams)
				{
					List<double> list = new List<double>();
					using (VolumeSlicer slicer = new VolumeSlicer(volume, slicing, DicomUid.GenerateUid().UID))
					{
						foreach (ISopDataSource slice in slicer.CreateSlices())
						{
							using (ImageSop imageSop = new ImageSop(slice))
							{
								foreach (IPresentationImage image in PresentationImageFactory.Create(imageSop))
								{
									IImageSopProvider imageSopProvider = (IImageSopProvider) image;
									IImageGraphicProvider imageGraphicProvider = (IImageGraphicProvider) image;
									DicomImagePlane dip = DicomImagePlane.FromImage(image);

									for (int y = 1; y < imageSopProvider.Frame.Rows - 1; y++)
									{
										for (int x = 1; x < imageSopProvider.Frame.Columns - 1; x++)
										{
											// pixels on the extreme sides of the volume tend to have more interpolation error due to MPR padding values
											Vector3D vector = dip.ConvertToPatient(new PointF(x, y)); // +new Vector3D(-0.5f, -0.5f, 0);
											if (Between(vector.X, 1, 98) && Between(vector.Y, 1, 98) && Between(vector.Z, 1, 98))
											{
												float expected = volumeKernel.Invoke(normalizedFunction, vector.X, vector.Y, vector.Z) + offset;
												float actual = imageKernel.Invoke(imageGraphicProvider.ImageGraphic.PixelData, x, y);
												list.Add(Math.Abs(expected - actual));
											}
										}
									}

									image.Dispose();
								}
							}
							slice.Dispose();
						}
					}

					Statistics stats = new Statistics(list);
					Trace.WriteLine(string.Format("Testing {0}", testName));
					Trace.WriteLine(string.Format("\tFunction/Slicing: {0} / {1}", normalizedFunction.Name, slicing.Description));
					Trace.WriteLine(string.Format("\t       Pixel Rep: {0}", signed ? "signed" : "unsigned"));
					Trace.WriteLine(string.Format("\t Voxels Compared: {0}", list.Count));
					Trace.WriteLine(string.Format("\t      Mean Delta: {0:f2} ({1:p2} of full scale)", stats.Mean, stats.Mean/FULL_SCALE));
					Trace.WriteLine(string.Format("\t    StdDev Delta: {0:f2} ({1:p2} of full scale)", stats.StandardDeviation, stats.StandardDeviation/FULL_SCALE));
					Assert.Less(stats.Mean, FULL_SCALE*0.05, "Mean delta exceeds 5% of full scale ({0})", FULL_SCALE);
					Assert.Less(stats.StandardDeviation, FULL_SCALE*0.05, "StdDev delta exceeds 5% of full scale ({0})", FULL_SCALE);
				}
			}
		}

		private static bool Between(double value, double min, double max)
		{
			return value >= min && value <= max;
		}

		private delegate float ImageKernelFunction(PixelData pd, int x, int y);

		private static float ImageKernelFunction3X3(PixelData pd, int x, int y)
		{
			float weightedMean = 0;
			weightedMean += 0.0625f*pd.GetPixel(x - 1, y - 1);
			weightedMean += 0.0625f*pd.GetPixel(x - 1, y + 0);
			weightedMean += 0.0625f*pd.GetPixel(x - 1, y + 1);
			weightedMean += 0.0625f*pd.GetPixel(x + 0, y - 1);
			weightedMean += 0.5000f*pd.GetPixel(x + 0, y + 0);
			weightedMean += 0.0625f*pd.GetPixel(x + 0, y + 1);
			weightedMean += 0.0625f*pd.GetPixel(x + 1, y - 1);
			weightedMean += 0.0625f*pd.GetPixel(x + 1, y + 0);
			weightedMean += 0.0625f*pd.GetPixel(x + 1, y + 1);
			return weightedMean;
		}

		private delegate float VolumeKernelFunction(VolumeFunction function, float x, float y, float z);

		private static float VolumeKernelFunction3X3X3(VolumeFunction function, float x, float y, float z)
		{
			const float side = 0.5f/26;
			float weightedMean = 0;
			weightedMean += side*function.Evaluate(x - 1, y - 1, z - 1);
			weightedMean += side*function.Evaluate(x - 1, y - 1, z + 0);
			weightedMean += side*function.Evaluate(x - 1, y - 1, z + 1);
			weightedMean += side*function.Evaluate(x - 1, y + 0, z - 1);
			weightedMean += side*function.Evaluate(x - 1, y + 0, z + 0);
			weightedMean += side*function.Evaluate(x - 1, y + 0, z + 1);
			weightedMean += side*function.Evaluate(x - 1, y + 1, z - 1);
			weightedMean += side*function.Evaluate(x - 1, y + 1, z + 0);
			weightedMean += side*function.Evaluate(x - 1, y + 1, z + 1);

			weightedMean += side*function.Evaluate(x + 0, y - 1, z - 1);
			weightedMean += side*function.Evaluate(x + 0, y - 1, z + 0);
			weightedMean += side*function.Evaluate(x + 0, y - 1, z + 1);
			weightedMean += side*function.Evaluate(x + 0, y + 0, z - 1);
			weightedMean += 0.5f*function.Evaluate(x + 0, y + 0, z + 0);
			weightedMean += side*function.Evaluate(x + 0, y + 0, z + 1);
			weightedMean += side*function.Evaluate(x + 0, y + 1, z - 1);
			weightedMean += side*function.Evaluate(x + 0, y + 1, z + 0);
			weightedMean += side*function.Evaluate(x + 0, y + 1, z + 1);

			weightedMean += side*function.Evaluate(x + 1, y - 1, z - 1);
			weightedMean += side*function.Evaluate(x + 1, y - 1, z + 0);
			weightedMean += side*function.Evaluate(x + 1, y - 1, z + 1);
			weightedMean += side*function.Evaluate(x + 1, y + 0, z - 1);
			weightedMean += side*function.Evaluate(x + 1, y + 0, z + 0);
			weightedMean += side*function.Evaluate(x + 1, y + 0, z + 1);
			weightedMean += side*function.Evaluate(x + 1, y + 1, z - 1);
			weightedMean += side*function.Evaluate(x + 1, y + 1, z + 0);
			weightedMean += side*function.Evaluate(x + 1, y + 1, z + 1);
			return weightedMean;
		}
	}
}

#endif