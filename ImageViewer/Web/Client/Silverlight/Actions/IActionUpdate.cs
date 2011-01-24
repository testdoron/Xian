#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.ImageViewer.Web.Client.Silverlight.AppServiceReference;

namespace ClearCanvas.ImageViewer.Web.Client.Silverlight.Actions
{
	public interface IActionUpdate
	{
		void Update(PropertyChangedEvent e);
	}
}