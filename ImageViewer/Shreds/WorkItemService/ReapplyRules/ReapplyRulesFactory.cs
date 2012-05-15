﻿#region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Common.WorkItem;
using ClearCanvas.ImageViewer.StudyManagement.Core.WorkItemProcessor;

namespace ClearCanvas.ImageViewer.Shreds.WorkItemService.ReapplyRules
{
    [ExtensionOf(typeof(WorkItemProcessorFactoryExtensionPoint))]
    public class ReapplyRulesFactory : IWorkItemProcessorFactory
    {
        public string GetWorkQueueType()
        {
            return ReapplyRulesRequest.WorkItemTypeString;
        }

        public IWorkItemProcessor GetItemProcessor()
        {
            return new ReapplyRulesItemProcessor();
        }
    }
}
