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

using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.Annotations;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.AdvancedImaging.Fusion
{
	[Cloneable]
	public class FusionPresentationImage : BasicPresentationImage, IImageSopProvider, IColorMapProvider, ILayerOpacityProvider, IModalityLutProvider
	{
		private const string _fusionOverlayLayerName = "Fusion";

		[CloneIgnore]
		private IFrameReference _baseFrameReference;

		[CloneIgnore]
		private IFusionOverlayFrameDataReference _overlayFrameDataReference;

		[CloneIgnore]
		private FusionOverlayCompositeGraphic _fusionOverlayComposite;

		[CloneIgnore]
		private CompositeGraphic _fusionOverlayLayer;

		public FusionPresentationImage(Frame baseFrame, FusionOverlayFrameData overlayData)
			: this(baseFrame.CreateTransientReference(), overlayData.CreateTransientReference()) {}

		public FusionPresentationImage(IFrameReference baseFrame, IFusionOverlayFrameDataReference overlayFrameData)
			: base(Create(baseFrame),
			       baseFrame.Frame.NormalizedPixelSpacing.Column,
			       baseFrame.Frame.NormalizedPixelSpacing.Row,
			       baseFrame.Frame.PixelAspectRatio.Column,
			       baseFrame.Frame.PixelAspectRatio.Row)
		{
			_baseFrameReference = baseFrame;
			_overlayFrameDataReference = overlayFrameData;

			Initialize();
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="source">The source object from which to clone.</param>
		/// <param name="context">The cloning context object.</param>
		protected FusionPresentationImage(FusionPresentationImage source, ICloningContext context) : base(source, context)
		{
			context.CloneFields(source, this);

			_baseFrameReference = source._baseFrameReference.Clone();
			_overlayFrameDataReference = source._overlayFrameDataReference.Clone();
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			_fusionOverlayLayer = (CompositeGraphic) CollectionUtils.SelectFirst(base.CompositeImageGraphic.Graphics,
			                                                                     g => g is CompositeGraphic && g.Name == _fusionOverlayLayerName);

			if (_fusionOverlayLayer != null)
			{
				_fusionOverlayComposite = (FusionOverlayCompositeGraphic) CollectionUtils.SelectFirst(_fusionOverlayLayer.Graphics, g => g is FusionOverlayCompositeGraphic);
			}

			Initialize();
		}

		private void Initialize()
		{
			if (_fusionOverlayLayer == null)
			{
				_fusionOverlayLayer = new CompositeGraphic {Name = _fusionOverlayLayerName};

				// insert the fusion graphics layer right after the base image graphic (both contain domain-level graphics)
				base.CompositeImageGraphic.Graphics.Insert(base.CompositeImageGraphic.Graphics.IndexOf(this.ImageGraphic) + 1, _fusionOverlayLayer);
			}

			if (_fusionOverlayComposite == null)
			{
				_fusionOverlayComposite = new FusionOverlayCompositeGraphic(_overlayFrameDataReference.FusionOverlayFrameData);
				_fusionOverlayLayer.Graphics.Add(_fusionOverlayComposite);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_fusionOverlayLayer = null;
				_fusionOverlayComposite = null;

				if (_baseFrameReference != null)
				{
					_baseFrameReference.Dispose();
					_baseFrameReference = null;
				}

				if (_overlayFrameDataReference != null)
				{
					_overlayFrameDataReference.Dispose();
					_overlayFrameDataReference = null;
				}
			}

			base.Dispose(disposing);
		}

		public FusionOverlayFrameData OverlayFrameData
		{
			get { return _overlayFrameDataReference.FusionOverlayFrameData; }
		}

		/// <summary>
		/// Gets the <see cref="GrayscaleImageGraphic"/> associated with this <see cref="GrayscalePresentationImage"/>.
		/// </summary>
		public new GrayscaleImageGraphic ImageGraphic
		{
			get { return (GrayscaleImageGraphic) base.ImageGraphic; }
		}

		public IVoiLutManager BaseVoiLutManager
		{
			get { return ImageGraphic.VoiLutManager; }
		}

		public IVoiLutManager OverlayVoiLutManager
		{
			get { return _fusionOverlayComposite.VoiLutManager; }
		}

		public override IPresentationImage CreateFreshCopy()
		{
			return new FusionPresentationImage(this.Frame, _fusionOverlayComposite.OverlayFrameData) {PresentationState = this.PresentationState};
		}

		/// <summary>
		/// Creates the <see cref="IAnnotationLayout"/> for this image.
		/// </summary>
		/// <returns></returns>
		protected override IAnnotationLayout CreateAnnotationLayout()
		{
			return AnnotationLayoutFactory.CreateLayout("AdvancedImaging.Fusion.PET-CT");
		}

		public override string ToString()
		{
			return Frame.ParentImageSop.InstanceNumber.ToString();
		}

		#region VOI LUT Synchronization Support

		private object _lastBaseVoiLutManagerMemento;
		private object _lastOverlayVoiLutManagerMemento;

		internal bool SetBaseVoiLutManagerMemento(object memento)
		{
			if (!Equals(memento, _lastBaseVoiLutManagerMemento))
			{
				ImageGraphic.VoiLutManager.SetMemento(_lastBaseVoiLutManagerMemento = memento);
				return true;
			}
			return false;
		}

		internal bool SetOverlayVoiLutManagerMemento(object memento)
		{
			if (!Equals(memento, _lastOverlayVoiLutManagerMemento))
			{
				_fusionOverlayComposite.VoiLutManager.SetMemento(_lastOverlayVoiLutManagerMemento = memento);
				return true;
			}
			return false;
		}

		#endregion

		#region IImageSopProvider Members

		/// <summary>
		/// Gets this presentation image's associated <see cref="ImageSop"/>.
		/// </summary>
		/// <remarks>
		/// Use <see cref="ImageSop"/> to access DICOM tags.
		/// </remarks>
		public ImageSop ImageSop
		{
			get { return Frame.ParentImageSop; }
		}

		/// <summary>
		/// Gets this presentation image's associated <see cref="Frame"/>.
		/// </summary>
		public Frame Frame
		{
			get { return _baseFrameReference.Frame; }
		}

		#endregion

		#region ISopProvider Members

		Sop ISopProvider.Sop
		{
			get { return ImageSop; }
		}

		#endregion

		#region IColorMapProvider Members

		public IColorMapManager ColorMapManager
		{
			get { return _fusionOverlayComposite.ColorMapManager; }
		}

		#endregion

		#region ILayerOpacityProvider Members

		public ILayerOpacityManager LayerOpacityManager
		{
			get { return _fusionOverlayComposite.LayerOpacityManager; }
		}

		#endregion

		#region IModalityLutProvider Members

		public IComposableLut ModalityLut
		{
			get { return ImageGraphic.ModalityLut; }
		}

		#endregion

		#region Private Helpers

		//TODO (CR Sept 2010): Name - Create implies it will create an instance of FusionPresentationImage.
		private static GrayscaleImageGraphic Create(IImageSopProvider frameReference)
		{
			return new GrayscaleImageGraphic(
				frameReference.Frame.Rows,
				frameReference.Frame.Columns,
				frameReference.Frame.BitsAllocated,
				frameReference.Frame.BitsStored,
				frameReference.Frame.HighBit,
				frameReference.Frame.PixelRepresentation != 0 ? true : false,
				frameReference.Frame.PhotometricInterpretation == PhotometricInterpretation.Monochrome1 ? true : false,
				frameReference.Frame.RescaleSlope,
				frameReference.Frame.RescaleIntercept,
				frameReference.Frame.GetNormalizedPixelData);
		}

		#endregion
	}
}