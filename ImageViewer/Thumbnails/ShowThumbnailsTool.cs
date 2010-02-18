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
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.BaseTools;
using System;
using ClearCanvas.ImageViewer.Thumbnails.Configuration;

namespace ClearCanvas.ImageViewer.Thumbnails
{
	[MenuAction("show", "global-menus/MenuView/MenuShowThumbnails", "Show")]
	[ButtonAction("show", "global-toolbars/ToolbarStandard/ToolbarShowThumbnails", "Show")]
	[Tooltip("show", "TooltipShowThumbnails")]
	[IconSet("show", IconScheme.Colour, "Icons.ShowThumbnailsToolSmall.png", "Icons.ShowThumbnailsToolMedium.png", "Icons.ShowThumbnailsToolLarge.png")]
	[EnabledStateObserver("show", "Enabled", "EnabledChanged")]

	[ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
	public class ShowThumbnailsTool : ImageViewerTool
	{
		private static readonly Dictionary<IDesktopWindow, IShelf> _shelves = new Dictionary<IDesktopWindow, IShelf>();

		public ShowThumbnailsTool()
		{
		}

		private IShelf ComponentShelf
		{
			get
			{
				if (_shelves.ContainsKey(this.Context.DesktopWindow))
					return _shelves[this.Context.DesktopWindow];

				return null;
			}	
		}

		public void Show()
		{
			if (ComponentShelf == null)
			{
				try
				{
					IDesktopWindow desktopWindow = this.Context.DesktopWindow;

					IShelf shelf = ThumbnailComponent.Launch(desktopWindow);
					shelf.Closed += delegate
					                	{
					                		_shelves.Remove(desktopWindow);
					                	};

					_shelves[this.Context.DesktopWindow] = shelf;
				}
				catch(Exception e)
				{
					ExceptionHandler.Report(e, this.Context.DesktopWindow);
				}
			}
			else
			{
				ComponentShelf.Show();
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			if (ThumbnailsSettings.Default.AutoOpenThumbnails)
			{
				this.Show();
			}
		}
	}
}
