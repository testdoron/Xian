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
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Graphics;

namespace ClearCanvas.ImageViewer.PresentationStates.Dicom
{
	/// <summary>
	/// The composite graphic containing all the DICOM-defined graphics that may be specified in
	/// a DICOM image header or an accompanying presentation state.
	/// </summary>
	/// <remarks>
	/// <para>This graphic provides explicit support for the following DICOM graphics concepts:</para>
	/// <list type="table">
	/// <listheader><dicom>Reference</dicom><description>Module</description></listheader>
	/// <item><dicom>PS 3.3 C.7.6.11</dicom><description>Display Shutter</description></item>
	/// <item><dicom>PS 3.3 C.7.6.15</dicom><description>Bitmap Display Shutter</description></item>
	/// <item><dicom>PS 3.3 C.9.2</dicom><description>Overlay Plane</description></item>
	/// <item><dicom>PS 3.3 C.11.7</dicom><description>Overlay Activation</description></item>
	/// <item><dicom>PS 3.3 C.10.7</dicom><description>Graphic Layer</description></item>
	/// </list>
	/// <para>Additionally, support for the following DICOM is available by directly using the appropriate class:</para>
	/// <list type="table">
	/// <listheader><dicom>Reference</dicom><description>Module</description><class>See</class></listheader>
	/// <item><dicom>PS 3.3 C.10.5</dicom><description>Graphic Annotation</description><class><see cref="DicomGraphicAnnotation"/></class></item>
	/// </list>
	/// </remarks>
	[Cloneable(true)]
	public partial class DicomGraphicsPlane : CompositeGraphic
	{
		[CloneIgnore]
		private readonly OverlaysCollection _imageOverlays;

		[CloneIgnore]
		private readonly OverlaysCollection _presentationOverlays;

		[CloneIgnore]
		private readonly UserOverlaysCollection _userOverlays;

		[CloneIgnore]
		private ShutterCollection _shutter;

		[CloneIgnore]
		private LayerCollection _layers;

		/// <summary>
		/// Constructs a new instance of a <see cref="DicomGraphicsPlane"/>.
		/// </summary>
		public DicomGraphicsPlane()
		{
			_imageOverlays = new OverlaysCollection(this);
			_presentationOverlays = new OverlaysCollection(this);
			_userOverlays = new UserOverlaysCollection(this);

			base.Graphics.Add(_shutter = new ShutterCollection());
			base.Graphics.Add(_layers = new LayerCollection());
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			if (_shutter != null)
			{
				base.Graphics.Remove(_shutter);
				_shutter.Dispose();
				_shutter = null;
			}

			if (_layers != null)
			{
				base.Graphics.Remove(_layers);
				_layers.Dispose();
				_layers = null;
			}

			_shutter = (ShutterCollection) CollectionUtils.SelectFirst(base.Graphics, IsType<ShutterCollection>);
			_layers = (LayerCollection) CollectionUtils.SelectFirst(base.Graphics, IsType<LayerCollection>);

			FillOverlayCollections(_shutter);
			foreach (LayerGraphic layer in _layers)
				FillOverlayCollections(layer.Graphics);
		}

		private void FillOverlayCollections(IEnumerable<IGraphic> collection)
		{
			foreach (OverlayPlaneGraphic overlay in CollectionUtils.Select(collection, IsType<OverlayPlaneGraphic>))
			{
				if (overlay.Source == OverlayPlaneSource.Image)
					_imageOverlays.Add(overlay);
				else if (overlay.Source == OverlayPlaneSource.PresentationState)
					_presentationOverlays.Add(overlay);
				else
					_userOverlays.Add(overlay);
			}
		}

		/// <summary>
		/// Clears all graphics from the DICOM graphics plane.
		/// </summary>
		public void Clear()
		{
			_shutter.Clear();
			_layers.Clear();
			_imageOverlays.Clear();
			_presentationOverlays.Clear();
			_userOverlays.Clear();
		}

		/// <summary>
		/// Gets a collection of available shutters.
		/// </summary>
		public IDicomGraphicsPlaneShutters Shutters
		{
			get { return _shutter; }
		}

		/// <summary>
		/// Gets a collection of available graphic layers.
		/// </summary>
		public IDicomGraphicsPlaneLayers Layers
		{
			get { return _layers; }
		}

		/// <summary>
		/// Gets a collection of available overlays from the image SOP.
		/// </summary>
		/// <remarks>
		/// The indices of overlays in this collection are restricted to 0-15, representing the 16 available overlay plane groups.
		/// </remarks>
		public IDicomGraphicsPlaneOverlays ImageOverlays
		{
			get { return _imageOverlays; }
		}

		/// <summary>
		/// Gets a collection of available overlays from an associated presentation state SOP, if one exists.
		/// </summary>
		/// <remarks>
		/// The indices of overlays in this collection are restricted to 0-15, representing the 16 available overlay plane groups.
		/// </remarks>
		public IDicomGraphicsPlaneOverlays PresentationOverlays
		{
			get { return _presentationOverlays; }
		}

		/// <summary>
		/// Gets a collection of existing user-created overlays.
		/// </summary>
		/// <remarks>
		/// This collection gives client code a collection to insert any dynamically created overlay planes which may not necessarily
		/// have an assigned overlay plane group. During DICOM softcopy presentation state serialization, visible overlays in this collection
		/// will be given priority for serialization, since only 16 overlays can be serialized.
		/// </remarks>
		public IDicomGraphicsPlaneOverlays UserOverlays
		{
			get { return _userOverlays; }
		}

		private static bool IsType<T>(object test)
		{
			return test is T;
		}

		/// <summary>
		/// Gets the DICOM graphics plane of the specified DICOM presentation image, creating the plane if necessary.
		/// </summary>
		/// <param name="dicomPresentationImage">The target DICOM presentation image.</param>
		/// <returns>The DICOM graphics plane associated with the specified presentation image.</returns>
		public static DicomGraphicsPlane GetDicomGraphicsPlane(IDicomPresentationImage dicomPresentationImage)
		{
			return GetDicomGraphicsPlane(dicomPresentationImage, true);
		}

		/// <summary>
		/// Gets the DICOM graphics plane of the specified DICOM presentation image.
		/// </summary>
		/// <param name="dicomPresentationImage">The target DICOM presentation image.</param>
		/// <param name="createIfNecessary">A value indicating if a DICOM graphics plane should be automatically created and associated with the presentation image.</param>
		/// <returns>The DICOM graphics plane associated with the specified presentation image, or null if one doesn't exist and was not created.</returns>
		public static DicomGraphicsPlane GetDicomGraphicsPlane(IDicomPresentationImage dicomPresentationImage, bool createIfNecessary)
		{
			if (dicomPresentationImage == null)
				return null;

			GraphicCollection dicomGraphics = dicomPresentationImage.DicomGraphics;
			DicomGraphicsPlane dicomGraphicsPlane = CollectionUtils.SelectFirst(dicomGraphics, IsType<DicomGraphicsPlane>) as DicomGraphicsPlane;

			if (dicomGraphicsPlane == null && createIfNecessary)
				dicomGraphics.Add(dicomGraphicsPlane = new DicomGraphicsPlane());

			return dicomGraphicsPlane;
		}
	}
}