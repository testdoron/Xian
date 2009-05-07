#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class ApplicationLog: ServerEntity
    {
        #region Constructors
        public ApplicationLog():base("ApplicationLog")
        {}
        public ApplicationLog(
             System.String _exception_
            ,System.String _host_
            ,System.String _logLevel_
            ,System.String _message_
            ,System.String _thread_
            ,System.DateTime _timestamp_
            ):base("ApplicationLog")
        {
            _exception = _exception_;
            _host = _host_;
            _logLevel = _logLevel_;
            _message = _message_;
            _thread = _thread_;
            _timestamp = _timestamp_;
        }
        #endregion

        #region Private Members
        private String _exception;
        private String _host;
        private String _logLevel;
        private String _message;
        private String _thread;
        private DateTime _timestamp;
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Exception")]
        public String Exception
        {
        get { return _exception; }
        set { _exception = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Host")]
        public String Host
        {
        get { return _host; }
        set { _host = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="LogLevel")]
        public String LogLevel
        {
        get { return _logLevel; }
        set { _logLevel = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Message")]
        public String Message
        {
        get { return _message; }
        set { _message = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Thread")]
        public String Thread
        {
        get { return _thread; }
        set { _thread = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Timestamp")]
        public DateTime Timestamp
        {
        get { return _timestamp; }
        set { _timestamp = value; }
        }
        #endregion

        #region Static Methods
        static public ApplicationLog Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public ApplicationLog Load(IPersistenceContext read, ServerEntityKey key)
        {
            IApplicationLogEntityBroker broker = read.GetBroker<IApplicationLogEntityBroker>();
            ApplicationLog theObject = broker.Load(key);
            return theObject;
        }
        static public ApplicationLog Insert(ApplicationLog entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                ApplicationLog newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public ApplicationLog Insert(IUpdateContext update, ApplicationLog entity)
        {
            IApplicationLogEntityBroker broker = update.GetBroker<IApplicationLogEntityBroker>();
            ApplicationLogUpdateColumns updateColumns = new ApplicationLogUpdateColumns();
            updateColumns.Exception = entity.Exception;
            updateColumns.Host = entity.Host;
            updateColumns.LogLevel = entity.LogLevel;
            updateColumns.Message = entity.Message;
            updateColumns.Thread = entity.Thread;
            updateColumns.Timestamp = entity.Timestamp;
            ApplicationLog newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
