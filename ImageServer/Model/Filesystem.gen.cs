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
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class Filesystem: ServerEntity
    {
        #region Constructors
        public Filesystem():base("Filesystem")
        {}
        public Filesystem(
             String _filesystemPath_
            ,Boolean _enabled_
            ,Boolean _readOnly_
            ,Boolean _writeOnly_
            ,FilesystemTierEnum _filesystemTierEnum_
            ,Decimal _lowWatermark_
            ,Decimal _highWatermark_
            ,String _description_
            ):base("Filesystem")
        {
            FilesystemPath = _filesystemPath_;
            Enabled = _enabled_;
            ReadOnly = _readOnly_;
            WriteOnly = _writeOnly_;
            FilesystemTierEnum = _filesystemTierEnum_;
            LowWatermark = _lowWatermark_;
            HighWatermark = _highWatermark_;
            Description = _description_;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="FilesystemPath")]
        public String FilesystemPath
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="Enabled")]
        public Boolean Enabled
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="ReadOnly")]
        public Boolean ReadOnly
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="WriteOnly")]
        public Boolean WriteOnly
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="FilesystemTierEnum")]
        public FilesystemTierEnum FilesystemTierEnum
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="LowWatermark")]
        public Decimal LowWatermark
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="HighWatermark")]
        public Decimal HighWatermark
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Filesystem", ColumnName="Description")]
        public String Description
        { get; set; }
        #endregion

        #region Static Methods
        static public Filesystem Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public Filesystem Load(IPersistenceContext read, ServerEntityKey key)
        {
            IFilesystemEntityBroker broker = read.GetBroker<IFilesystemEntityBroker>();
            Filesystem theObject = broker.Load(key);
            return theObject;
        }
        static public Filesystem Insert(Filesystem entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                Filesystem newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public Filesystem Insert(IUpdateContext update, Filesystem entity)
        {
            IFilesystemEntityBroker broker = update.GetBroker<IFilesystemEntityBroker>();
            FilesystemUpdateColumns updateColumns = new FilesystemUpdateColumns();
            updateColumns.FilesystemPath = entity.FilesystemPath;
            updateColumns.Enabled = entity.Enabled;
            updateColumns.ReadOnly = entity.ReadOnly;
            updateColumns.WriteOnly = entity.WriteOnly;
            updateColumns.FilesystemTierEnum = entity.FilesystemTierEnum;
            updateColumns.LowWatermark = entity.LowWatermark;
            updateColumns.HighWatermark = entity.HighWatermark;
            updateColumns.Description = entity.Description;
            Filesystem newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
