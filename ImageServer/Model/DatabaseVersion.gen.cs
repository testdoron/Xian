#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0//

#endregion

// This file is auto-generated by the ClearCanvas.Model.SqlServer.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Xml;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class DatabaseVersion: ServerEntity
    {
        #region Constructors
        public DatabaseVersion():base("DatabaseVersion_")
        {}
        public DatabaseVersion(
             String _major__
            ,String _minor__
            ,String _build__
            ,String _revision__
            ):base("DatabaseVersion_")
        {
            Major = _major__;
            Minor = _minor__;
            Build = _build__;
            Revision = _revision__;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="DatabaseVersion_", ColumnName="Major_")]
        public String Major
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="DatabaseVersion_", ColumnName="Minor_")]
        public String Minor
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="DatabaseVersion_", ColumnName="Build_")]
        public String Build
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="DatabaseVersion_", ColumnName="Revision_")]
        public String Revision
        { get; set; }
        #endregion

        #region Static Methods
        static public DatabaseVersion Load(ServerEntityKey key)
        {
            using (var read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public DatabaseVersion Load(IPersistenceContext read, ServerEntityKey key)
        {
            var broker = read.GetBroker<IDatabaseVersionEntityBroker>();
            DatabaseVersion theObject = broker.Load(key);
            return theObject;
        }
        static public DatabaseVersion Insert(DatabaseVersion entity)
        {
            using (var update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                DatabaseVersion newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public DatabaseVersion Insert(IUpdateContext update, DatabaseVersion entity)
        {
            var broker = update.GetBroker<IDatabaseVersionEntityBroker>();
            var updateColumns = new DatabaseVersionUpdateColumns();
            updateColumns.Major = entity.Major;
            updateColumns.Minor = entity.Minor;
            updateColumns.Build = entity.Build;
            updateColumns.Revision = entity.Revision;
            DatabaseVersion newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
