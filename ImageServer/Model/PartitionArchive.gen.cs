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
    public partial class PartitionArchive: ServerEntity
    {
        #region Constructors
        public PartitionArchive():base("PartitionArchive")
        {}
        public PartitionArchive(
             ServerEntityKey _serverPartitionKey_
            ,ArchiveTypeEnum _archiveTypeEnum_
            ,String _description_
            ,Boolean _enabled_
            ,Boolean _readOnly_
            ,Int32 _archiveDelayHours_
            ,XmlDocument _configurationXml_
            ):base("PartitionArchive")
        {
            ServerPartitionKey = _serverPartitionKey_;
            ArchiveTypeEnum = _archiveTypeEnum_;
            Description = _description_;
            Enabled = _enabled_;
            ReadOnly = _readOnly_;
            ArchiveDelayHours = _archiveDelayHours_;
            ConfigurationXml = _configurationXml_;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="ServerPartitionGUID")]
        public ServerEntityKey ServerPartitionKey
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="ArchiveTypeEnum")]
        public ArchiveTypeEnum ArchiveTypeEnum
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="Description")]
        public String Description
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="Enabled")]
        public Boolean Enabled
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="ReadOnly")]
        public Boolean ReadOnly
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="ArchiveDelayHours")]
        public Int32 ArchiveDelayHours
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionArchive", ColumnName="ConfigurationXml")]
        public XmlDocument ConfigurationXml
        { get; set; }
        #endregion

        #region Static Methods
        static public PartitionArchive Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public PartitionArchive Load(IPersistenceContext read, ServerEntityKey key)
        {
            IPartitionArchiveEntityBroker broker = read.GetBroker<IPartitionArchiveEntityBroker>();
            PartitionArchive theObject = broker.Load(key);
            return theObject;
        }
        static public PartitionArchive Insert(PartitionArchive entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                PartitionArchive newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public PartitionArchive Insert(IUpdateContext update, PartitionArchive entity)
        {
            IPartitionArchiveEntityBroker broker = update.GetBroker<IPartitionArchiveEntityBroker>();
            PartitionArchiveUpdateColumns updateColumns = new PartitionArchiveUpdateColumns();
            updateColumns.ServerPartitionKey = entity.ServerPartitionKey;
            updateColumns.ArchiveTypeEnum = entity.ArchiveTypeEnum;
            updateColumns.Description = entity.Description;
            updateColumns.Enabled = entity.Enabled;
            updateColumns.ReadOnly = entity.ReadOnly;
            updateColumns.ArchiveDelayHours = entity.ArchiveDelayHours;
            updateColumns.ConfigurationXml = entity.ConfigurationXml;
            PartitionArchive newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
