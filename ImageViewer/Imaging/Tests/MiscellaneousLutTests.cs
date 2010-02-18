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

using NUnit.Framework;

namespace ClearCanvas.ImageViewer.Imaging.Tests
{

	[TestFixture]
	public class MiscellaneousLutTests
	{
		public MiscellaneousLutTests()
		{
		}

		[Test]
		public void TestSimpleLut()
		{
			int bitsStored = 8;
			bool isSigned = false;
			double rescaleSlope = 0.5;
			double rescaleIntercept = 10;

			ModalityLutLinear modalityLUT = new ModalityLutLinear(
				bitsStored,
				isSigned,
				rescaleSlope,
				rescaleIntercept);

			SimpleDataLut simpleLut = 
				new SimpleDataLut(modalityLUT.MinInputValue, modalityLUT.Data, modalityLUT.MinOutputValue, modalityLUT.MaxOutputValue, modalityLUT.GetKey(), modalityLUT.GetDescription()); 

			Assert.AreEqual(modalityLUT.MinInputValue, simpleLut.MinInputValue);
			Assert.AreEqual(modalityLUT.MaxInputValue, simpleLut.MaxInputValue);
			Assert.AreEqual(modalityLUT.MinOutputValue, simpleLut.MinOutputValue);
			Assert.AreEqual(modalityLUT.MaxOutputValue, simpleLut.MaxOutputValue);
			Assert.AreEqual(modalityLUT.GetKey(), simpleLut.GetKey());
			Assert.AreEqual(modalityLUT.GetDescription(), simpleLut.GetDescription());
			Assert.AreEqual(modalityLUT.Length, simpleLut.Length);
		}
	}
}

#endif