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

using System.Collections.Generic;

namespace ClearCanvas.ImageViewer
{
	internal class LinkedImageEnumerator : IEnumerable<IPresentationImage>
	{
		private readonly IPresentationImage _referenceImage;
		private bool _includeAllImageSets;
		private bool _excludeReferenceImage;

		public LinkedImageEnumerator(IPresentationImage referenceImage)
		{
			_referenceImage = referenceImage;
			_includeAllImageSets = false;
			_excludeReferenceImage = false;
		}

		public bool IncludeAllImageSets
		{
			get { return _includeAllImageSets; }
			set { _includeAllImageSets = value; }
		}

		public bool ExcludeReferenceImage
		{
			get { return _excludeReferenceImage; }
			set { _excludeReferenceImage = value; }
		}

		private IEnumerable<IPresentationImage> GetAllLinkedImages()
		{
			IDisplaySet parentDisplaySet = _referenceImage.ParentDisplaySet;
			IImageSet parentImageSet = parentDisplaySet.ParentImageSet;

			// If display set is linked and selected, then iterate through all the linked images
			// from the other linked display sets
			if (parentDisplaySet.Linked)
			{
				if (_includeAllImageSets)
				{
					foreach (IImageSet imageSet in parentImageSet.ParentLogicalWorkspace.ImageSets)
					{
						foreach (IDisplaySet displaySet in imageSet.LinkedDisplaySets)
						{
							foreach (IPresentationImage image in GetAllLinkedImages(displaySet))
								yield return image;
						}
					}
				}
				else
				{
					foreach (IDisplaySet currentDisplaySet in parentImageSet.LinkedDisplaySets)
					{
						foreach (IPresentationImage image in GetAllLinkedImages(currentDisplaySet))
							yield return image;
					}
				}
			}
			// If display set is just selected, then iterate through all the linked images
			// in that display set.
			else
			{
				foreach (IPresentationImage image in GetAllLinkedImages(parentDisplaySet))
					yield return image;
			}
		}

		private IEnumerable<IPresentationImage> GetAllLinkedImages(IDisplaySet displaySet)
		{
			foreach (IPresentationImage image in displaySet.LinkedPresentationImages)
			{
				if (image != _referenceImage)
					yield return image;
			}
		}

		private IEnumerable<IPresentationImage> GetImages()
		{
			if (!_excludeReferenceImage)
				yield return _referenceImage;

			foreach (IPresentationImage image in this.GetAllLinkedImages())
				yield return image;
		}

		#region IEnumerable<IPresentationImage> Members

		public IEnumerator<IPresentationImage> GetEnumerator()
		{
			return GetImages().GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetImages().GetEnumerator();
		}

		#endregion
	}
}
