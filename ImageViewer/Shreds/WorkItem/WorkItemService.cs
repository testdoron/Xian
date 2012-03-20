﻿#region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Common.WorkItem;
using ClearCanvas.ImageViewer.StudyManagement.Storage;

namespace ClearCanvas.ImageViewer.Shreds.WorkItem
{
    public class WorkItemService : IWorkItemService
    {
        private static WorkItemService _instance;
        private bool _disabled;

        public static WorkItemService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WorkItemService();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        private void Initialize()
        {
            try
            {
             
            }
            catch (Exception e)
            {
                Platform.Log(LogLevel.Error, e);
                _disabled = true;
            }
        }

        public void Start()
        {
            CheckDisabled();          
          
            try
            {
            }
            catch (Exception e)
            {
                Platform.Log(LogLevel.Warn, e, "Failed to start purge timer; old items will never be purged.");
            }
        }

        public void Stop()
        {
            CheckDisabled();
   
        }

        private void CheckDisabled()
        {
            if (_disabled)
                throw new Exception(SR.ExceptionServiceHasBeenDisabled);
        }

        public WorkItemInsertResponse Insert(WorkItemInsertRequest request)
        {
            var response = new WorkItemInsertResponse();

            using (var context = new DataAccessContext())
            {
                var item = new StudyManagement.Storage.WorkItem
                               {
                                   Request = request.Request,
                                   Type = request.Request.Type,
                                   Priority = request.Request.Priority,
                                   InsertTime = Platform.Time,
                                   DeleteTime = Platform.Time,
                                   ExpirationTime = Platform.Time,
                                   ScheduledTime = Platform.Time,
                                   Status = WorkItemStatusEnum.Pending
                               };

                var broker = context.GetWorkItemBroker();
                broker.Insert(item);
                
                context.Commit();

                response.Item = WorkItemHelper.FromWorkItem(item);
            }

            return response;
        }

        public WorkItemUpdateResponse Update(WorkItemUpdateRequest request)
        {
            var response = new WorkItemUpdateResponse();
            using (var context = new DataAccessContext())
            {
                var broker = context.GetWorkItemBroker();
                var workItem = broker.GetWorkItem(request.Identifier);

                if (request.ExpirationTime.HasValue)
                    workItem.ExpirationTime = request.ExpirationTime.Value;
                if (request.Priority.HasValue)
                    workItem.Priority = request.Priority.Value;
                if (request.ScheduledTime.HasValue)
                    workItem.ScheduledTime = request.ScheduledTime.Value;
                
                if (request.Cancel.HasValue && request.Cancel.Value)
                {
                
                    if (workItem.Status.Equals(WorkItemStatusEnum.Idle)
                        ||workItem.Status.Equals(WorkItemStatusEnum.Pending))
                        workItem.Status = WorkItemStatusEnum.Canceled;
                    else if (workItem.Status.Equals(WorkItemStatusEnum.InProgress))
                    {
                        // Abort the WorkItem
                    }
                }
                context.Commit();
            }
            return response;
        }

        public WorkItemQueryResponse Query(WorkItemQueryRequest request)
        {
            var response = new WorkItemQueryResponse();
            using (var context = new DataAccessContext())
            {
                var broker = context.GetWorkItemBroker();
 
                var dbList = broker.GetWorkItems(request.Type, request.Status, request.StudyInstanceUid);

                var results = new List<WorkItemData>();

                foreach (var dbItem in dbList)
                {
                    results.Add(WorkItemHelper.FromWorkItem(dbItem));
                }

                response.Items = results.ToArray();
            }
            return response;
        }

        public WorkItemSubscribeResponse Subscribe(WorkItemSubscribeRequest request)
        {
            var response = new WorkItemSubscribeResponse();
            using (var context = new DataAccessContext())
            {
            }
            return response;
        }

        public WorkItemUnsubscribeResponse Unsubscribe(WorkItemUnsubscribeRequest type)
        {
            var response = new WorkItemUnsubscribeResponse();
            using (var context = new DataAccessContext())
            {
            }
            return response;
        }
    }
}