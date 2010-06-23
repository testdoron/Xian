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
using System.Drawing;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Volume.Mpr;
using VolumeData = ClearCanvas.ImageViewer.Volume.Mpr.Volume;

namespace ClearCanvas.ImageViewer.AdvancedImaging.Fusion.Tests
{
	public class TestDataFunction
	{
		/// <summary>
		/// The Threed function.
		/// </summary>
		/// <remarks>
		/// The Threed function consists of four non-co-planar spheres embedded in a volume.
		/// </remarks>
		public static TestDataFunction Threed = new TestDataFunction("Threed", (x, y, z) => UnitStep(5 - Distance(x, y, z, 25, 30, 35))
		                                                                                    + 2*UnitStep(5 - Distance(x, y, z, 50, 25, 50))
		                                                                                    + 3*UnitStep(5 - Distance(x, y, z, 63, 27, 36))
		                                                                                    + 4*UnitStep(5 - Distance(x, y, z, 79, 49, 65))).Normalize(100);

		private delegate float TestDataFunctionDelegate(float x, float y, float z);

		private readonly TestDataFunctionDelegate _function;
		private readonly string _name;
		private readonly string _studyInstanceUid;
		private readonly string _frameOfReferenceUid;

		private TestDataFunction(string name, TestDataFunctionDelegate function)
		{
			_function = function;
			_name = name;
			_studyInstanceUid = DicomUid.GenerateUid().UID;
			_frameOfReferenceUid = DicomUid.GenerateUid().UID;
		}

		public string Name
		{
			get { return _name; }
		}

		protected virtual float Evaluate(float x, float y, float z)
		{
			return _function(x, y, z);
		}

		protected float Evaluate(float x, float y, float z, Vector3D voxelSpacing)
		{
			return this.Evaluate(x*voxelSpacing.X, y*voxelSpacing.Y, z*voxelSpacing.Z);
		}

		private TestDataFunction Normalize(int side)
		{
			return Normalize(side, side, side);
		}

		private TestDataFunction Normalize(int width, int height, int depth)
		{
			return new NormalizedVolumeFunction(width, height, depth, this);
		}

		protected VolumeData CreateVolume(bool signed, Modality modality, Vector3D voxelSpacing)
		{
			Vector3D originPatient = new Vector3D(0, 0, 0);
			Matrix orientationPatient = Matrix.GetIdentity(4);
			int width, height, depth;
			if (signed)
			{
				short[] data = CreateSignedArray(out width, out height, out depth, voxelSpacing);
				DicomAttributeCollection dataset = CreateMockDataset(_name, _studyInstanceUid, _frameOfReferenceUid, modality, width, height, true, new SizeF(voxelSpacing.X, voxelSpacing.Y));
				Size3D dimensions = new Size3D(width, height, depth);
				return new VolumeData(data, dimensions, voxelSpacing, originPatient, orientationPatient, dataset, short.MinValue);
			}
			else
			{
				ushort[] data = CreateUnsignedArray(out width, out height, out depth, voxelSpacing);
				DicomAttributeCollection dataset = CreateMockDataset(_name, _studyInstanceUid, _frameOfReferenceUid, modality, width, height, false, new SizeF(voxelSpacing.X, voxelSpacing.Y));
				Size3D dimensions = new Size3D(width, height, depth);
				return new VolumeData(data, dimensions, voxelSpacing, originPatient, orientationPatient, dataset, ushort.MinValue);
			}
		}

		public ISopDataSource[] CreateSops(bool signed, Modality modality, Vector3D voxelSpacing, Vector3D sliceAxisX, Vector3D sliceAxisY, Vector3D sliceAxisZ)
		{
			var seriesInstanceUid = DicomUid.GenerateUid().UID;
			var slicerParams = new VolumeSlicerParams(sliceAxisX, sliceAxisY, sliceAxisZ);
			var volume = CreateVolume(signed, modality, voxelSpacing);
			using (VolumeSlicer slicer = new VolumeSlicer(volume, slicerParams, seriesInstanceUid))
			{
				return new List<ISopDataSource>(slicer.CreateSlices()).ToArray();
			}
		}

		protected short[] CreateSignedArray(out int width, out int height, out int depth, Vector3D voxelSpacing)
		{
			return Array.ConvertAll(CreateUnsignedArray(out width, out height, out depth, voxelSpacing), v => (short) (v - 32768));
		}

		protected ushort[] CreateUnsignedArray(out int width, out int height, out int depth, Vector3D voxelSpacing)
		{
			width = (int) (100/voxelSpacing.X);
			height = (int) (100/voxelSpacing.Y);
			depth = (int) (100/voxelSpacing.Z);

			ushort[] data = new ushort[width*height*depth];
			for (int z = 0; z < depth; z++)
			{
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						data[((z*height) + y)*width + x] = (ushort) Math.Max(Math.Min(this.Evaluate(x, y, z, voxelSpacing), ushort.MaxValue), ushort.MinValue);
					}
				}
			}
			return data;
		}

		private static DicomAttributeCollection CreateMockDataset(string patientName, string studyInstanceUid, string frameOfReferenceUid, Modality modality, int columns, int rows, bool signed, SizeF pixelSpacing)
		{
			DicomAttributeCollection dataset = new DicomAttributeCollection();
			dataset[DicomTags.PatientId].SetStringValue("PATIENT");
			dataset[DicomTags.PatientsName].SetStringValue(patientName);
			dataset[DicomTags.StudyId].SetStringValue("STUDY");
			dataset[DicomTags.SeriesDescription].SetStringValue(string.Format("SERIES-{0}", modality));
			dataset[DicomTags.StudyInstanceUid].SetStringValue(studyInstanceUid);
			dataset[DicomTags.SeriesInstanceUid].SetStringValue(DicomUid.GenerateUid().UID);
			dataset[DicomTags.SopInstanceUid].SetStringValue(DicomUid.GenerateUid().UID);
			dataset[DicomTags.SopClassUid].SetStringValue(GetSopClassUidByModality(modality));
			dataset[DicomTags.Modality].SetStringValue(modality.ToString());
			dataset[DicomTags.FrameOfReferenceUid].SetStringValue(frameOfReferenceUid);
			dataset[DicomTags.PixelSpacing].SetStringValue(string.Format(@"{0}\{1}", pixelSpacing.Height, pixelSpacing.Width));
			dataset[DicomTags.PhotometricInterpretation].SetStringValue("MONOCHROME2");
			dataset[DicomTags.BitsStored].SetInt32(0, 16);
			dataset[DicomTags.BitsAllocated].SetInt32(0, 16);
			dataset[DicomTags.HighBit].SetInt32(0, 15);
			dataset[DicomTags.PixelRepresentation].SetInt32(0, signed ? 1 : 0);
			dataset[DicomTags.Rows].SetInt32(0, rows);
			dataset[DicomTags.Columns].SetInt32(0, columns);
			dataset[DicomTags.WindowCenter].SetInt32(0, signed ? 0 : 32768);
			dataset[DicomTags.WindowWidth].SetInt32(0, 65536);
			dataset[DicomTags.WindowCenterWidthExplanation].SetString(0, "Full Window");
			return dataset;
		}

		private static string GetSopClassUidByModality(Modality modality)
		{
			switch (modality)
			{
				case Modality.CT:
					return SopClass.CtImageStorageUid;
				case Modality.MR:
					return SopClass.MrImageStorageUid;
				case Modality.PT:
					return SopClass.PositronEmissionTomographyImageStorageUid;
			}
			return SopClass.SecondaryCaptureImageStorageUid;
		}

		#region Standard Functions

		private static float SquarePulseTrain(float t, float period)
		{
			return Math.Abs((int) (2*t/period) + (t >= 0 ? 1 : 0))%2;
		}

		private static float UnitStep(float t)
		{
			return t >= 0 ? 1f : 0f;
		}

		private static float Distance(float x0, float y0, float x1, float y1)
		{
			float dx = x1 - x0;
			float dy = y1 - y0;
			return (float) Math.Sqrt(dx*dx + dy*dy);
		}

		private static float Distance(float x0, float y0, float z0, float x1, float y1, float z1)
		{
			float dx = x1 - x0;
			float dy = y1 - y0;
			float dz = z1 - z0;
			return (float) Math.Sqrt(dx*dx + dy*dy + dz*dz);
		}

		#endregion

		#region NormalizedVolumeFunction Class

		private class NormalizedVolumeFunction : TestDataFunction
		{
			private readonly float _offset;
			private readonly float _scale;

			public NormalizedVolumeFunction(int width, int height, int depth, TestDataFunction function)
				: base(function._name, function._function)
			{
				float rangeMin = float.MaxValue;
				float rangeMax = float.MinValue;
				for (int z = 0; z < depth; z++)
				{
					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							float v = function._function(x, y, z);
							rangeMin = Math.Min(rangeMin, v);
							rangeMax = Math.Max(rangeMax, v);
						}
					}
				}
				_offset = -rangeMin;
				_scale = 65535/(rangeMax - rangeMin);
			}

			protected override float Evaluate(float x, float y, float z)
			{
				return (base.Evaluate(x, y, z) + _offset)*_scale;
			}
		}

		#endregion

		#region SimpleSopDataSource Class

		private class SimpleSopDataSource : SopDataSource
		{
			private readonly DicomAttributeCollection _dataset;
			private readonly SimpleSopFrameData _frameData;

			public SimpleSopDataSource(DicomAttributeCollection dataset)
			{
				_dataset = dataset;
				_frameData = new SimpleSopFrameData(1, this);
			}

			public override bool IsImage
			{
				get { return true; }
			}

			protected override ISopFrameData GetFrameData(int frameNumber)
			{
				return _frameData;
			}

			public override DicomAttribute this[DicomTag tag]
			{
				get { return _dataset[tag]; }
			}

			public override DicomAttribute this[uint tag]
			{
				get { return _dataset[tag]; }
			}

			public override bool TryGetAttribute(DicomTag tag, out DicomAttribute attribute)
			{
				return _dataset.TryGetAttribute(tag, out attribute);
			}

			public override bool TryGetAttribute(uint tag, out DicomAttribute attribute)
			{
				return _dataset.TryGetAttribute(tag, out attribute);
			}

			private class SimpleSopFrameData : SopFrameData
			{
				public SimpleSopFrameData(int frameNumber, SopDataSource parent) : base(frameNumber, parent) {}

				public override byte[] GetNormalizedPixelData()
				{
					return (byte[]) base.Parent[DicomTags.PixelData].Values;
				}

				public override byte[] GetNormalizedOverlayData(int overlayGroupNumber, int overlayFrameNumber)
				{
					return new byte[0];
				}

				public override void Unload() {}
			}
		}

		#endregion
	}
}

#endif