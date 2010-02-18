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

using ClearCanvas.ImageViewer.Graphics;

namespace ClearCanvas.ImageViewer.Tools.Standard
{
	public partial class WindowLevelTool
	{
		private static readonly SensitivityMap Sensitivity = new SensitivityMap();

		private double CurrentSensitivity
		{
			get
			{
				if (base.SelectedImageGraphicProvider != null)
				{
					if (base.SelectedImageGraphicProvider.ImageGraphic is ColorImageGraphic)
						return Sensitivity[8];
					if (base.SelectedImageGraphicProvider.ImageGraphic is GrayscaleImageGraphic)
						return Sensitivity[((GrayscaleImageGraphic) base.SelectedImageGraphicProvider.ImageGraphic).BitsStored];
				}
				return 10;
			}
		}

		private class SensitivityMap
		{
			private static readonly double[] _increment;

			static SensitivityMap()
			{
				_increment = new double[16];
				_increment[0] = 0.05;
				_increment[1] = 0.1;
				_increment[2] = 0.5;
				_increment[3] = 0.5;
				_increment[4] = 1;
				_increment[5] = 1;
				_increment[6] = 1;
				_increment[7] = 1;
				_increment[8] = 5;
				_increment[9] = 5;
				_increment[10] = 5;
				_increment[11] = 5;
				_increment[12] = 10;
				_increment[13] = 10;
				_increment[14] = 10;
				_increment[15] = 10;
			}

			public double this[int bitsStored]
			{
				get
				{
					if (bitsStored > 16)
						bitsStored = 16;
					return _increment[bitsStored - 1];
				}
			}
		}
	}
}