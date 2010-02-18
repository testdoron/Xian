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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Xml;
    using ClearCanvas.Dicom;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class RequestAttributes: ServerEntity
    {
        #region Constructors
        public RequestAttributes():base("RequestAttributes")
        {}
        public RequestAttributes(
             ServerEntityKey _seriesKey_
            ,String _requestedProcedureId_
            ,String _scheduledProcedureStepId_
            ):base("RequestAttributes")
        {
            SeriesKey = _seriesKey_;
            RequestedProcedureId = _requestedProcedureId_;
            ScheduledProcedureStepId = _scheduledProcedureStepId_;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="RequestAttributes", ColumnName="SeriesGUID")]
        public ServerEntityKey SeriesKey
        { get; set; }
        [DicomField(DicomTags.RequestedProcedureId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="RequestAttributes", ColumnName="RequestedProcedureId")]
        public String RequestedProcedureId
        { get; set; }
        [DicomField(DicomTags.ScheduledProcedureStepId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="RequestAttributes", ColumnName="ScheduledProcedureStepId")]
        public String ScheduledProcedureStepId
        { get; set; }
        #endregion

        #region Static Methods
        static public RequestAttributes Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public RequestAttributes Load(IPersistenceContext read, ServerEntityKey key)
        {
            IRequestAttributesEntityBroker broker = read.GetBroker<IRequestAttributesEntityBroker>();
            RequestAttributes theObject = broker.Load(key);
            return theObject;
        }
        static public RequestAttributes Insert(RequestAttributes entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                RequestAttributes newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public RequestAttributes Insert(IUpdateContext update, RequestAttributes entity)
        {
            IRequestAttributesEntityBroker broker = update.GetBroker<IRequestAttributesEntityBroker>();
            RequestAttributesUpdateColumns updateColumns = new RequestAttributesUpdateColumns();
            updateColumns.SeriesKey = entity.SeriesKey;
            updateColumns.RequestedProcedureId = entity.RequestedProcedureId;
            updateColumns.ScheduledProcedureStepId = entity.ScheduledProcedureStepId;
            RequestAttributes newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
