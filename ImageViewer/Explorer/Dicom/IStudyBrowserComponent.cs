﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Services.ServerTree;

namespace ClearCanvas.ImageViewer.Explorer.Dicom
{
	[ExtensionPoint]
	public sealed class StudyBrowserComponentExtensionPoint : ExtensionPoint<IStudyBrowserComponent>
	{
	}

	public interface IStudyBrowserComponent : IApplicationComponent
	{
		QueryParameters OpenSearchQueryParams { get; }
		SearchResult CreateSearchResult();
		AEServerGroup SelectedServerGroup { get; set; }
		void Search(List<QueryParameters> queryParameters);
	}
}