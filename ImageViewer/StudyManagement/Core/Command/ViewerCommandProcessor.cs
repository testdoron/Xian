﻿#region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Dicom.Utilities.Command;

namespace ClearCanvas.ImageViewer.StudyManagement.Core.Command
{
    /// <summary>
    /// ImageViewer specific <see cref="CommandProcessor"/>.  This command processor takes into account how the ImageViewer accesses its database.
    /// </summary>
    public class ViewerCommandProcessor : CommandProcessor
    {
        public ViewerCommandProcessor(string description) 
            : base(description, new ViewerCommandProcessorContext())
        {}

        public ViewerCommandProcessorContext ViewerContext { get { return ProcessorContext as ViewerCommandProcessorContext; } }
    }
}
