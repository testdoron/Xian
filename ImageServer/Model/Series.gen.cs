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
    using ClearCanvas.Dicom;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class Series: ServerEntity
    {
        #region Constructors
        public Series():base("Series")
        {}
        public Series(
             System.String _modality_
            ,System.Int32 _numberOfSeriesRelatedInstances_
            ,System.String _performedProcedureStepStartDate_
            ,System.String _performedProcedureStepStartTime_
            ,System.String _seriesDescription_
            ,System.String _seriesInstanceUid_
            ,System.String _seriesNumber_
            ,ClearCanvas.ImageServer.Enterprise.ServerEntityKey _serverPartitionKey_
            ,System.String _sourceApplicationEntityTitle_
            ,ClearCanvas.ImageServer.Enterprise.ServerEntityKey _studyKey_
            ):base("Series")
        {
            _modality = _modality_;
            _numberOfSeriesRelatedInstances = _numberOfSeriesRelatedInstances_;
            _performedProcedureStepStartDate = _performedProcedureStepStartDate_;
            _performedProcedureStepStartTime = _performedProcedureStepStartTime_;
            _seriesDescription = _seriesDescription_;
            _seriesInstanceUid = _seriesInstanceUid_;
            _seriesNumber = _seriesNumber_;
            _serverPartitionKey = _serverPartitionKey_;
            _sourceApplicationEntityTitle = _sourceApplicationEntityTitle_;
            _studyKey = _studyKey_;
        }
        #endregion

        #region Private Members
        private String _modality;
        private Int32 _numberOfSeriesRelatedInstances;
        private String _performedProcedureStepStartDate;
        private String _performedProcedureStepStartTime;
        private String _seriesDescription;
        private String _seriesInstanceUid;
        private String _seriesNumber;
        private ServerEntityKey _serverPartitionKey;
        private String _sourceApplicationEntityTitle;
        private ServerEntityKey _studyKey;
        #endregion

        #region Public Properties
        [DicomField(DicomTags.Modality, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="Modality")]
        public String Modality
        {
        get { return _modality; }
        set { _modality = value; }
        }
        [DicomField(DicomTags.NumberOfSeriesRelatedInstances, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="NumberOfSeriesRelatedInstances")]
        public Int32 NumberOfSeriesRelatedInstances
        {
        get { return _numberOfSeriesRelatedInstances; }
        set { _numberOfSeriesRelatedInstances = value; }
        }
        [DicomField(DicomTags.PerformedProcedureStepStartDate, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="PerformedProcedureStepStartDate")]
        public String PerformedProcedureStepStartDate
        {
        get { return _performedProcedureStepStartDate; }
        set { _performedProcedureStepStartDate = value; }
        }
        [DicomField(DicomTags.PerformedProcedureStepStartTime, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="PerformedProcedureStepStartTime")]
        public String PerformedProcedureStepStartTime
        {
        get { return _performedProcedureStepStartTime; }
        set { _performedProcedureStepStartTime = value; }
        }
        [DicomField(DicomTags.SeriesDescription, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="SeriesDescription")]
        public String SeriesDescription
        {
        get { return _seriesDescription; }
        set { _seriesDescription = value; }
        }
        [DicomField(DicomTags.SeriesInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="SeriesInstanceUid")]
        public String SeriesInstanceUid
        {
        get { return _seriesInstanceUid; }
        set { _seriesInstanceUid = value; }
        }
        [DicomField(DicomTags.SeriesNumber, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="SeriesNumber")]
        public String SeriesNumber
        {
        get { return _seriesNumber; }
        set { _seriesNumber = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="ServerPartitionGUID")]
        public ServerEntityKey ServerPartitionKey
        {
        get { return _serverPartitionKey; }
        set { _serverPartitionKey = value; }
        }
        [DicomField(DicomTags.SourceApplicationEntityTitle, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="SourceApplicationEntityTitle")]
        public String SourceApplicationEntityTitle
        {
        get { return _sourceApplicationEntityTitle; }
        set { _sourceApplicationEntityTitle = value; }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Series", ColumnName="StudyGUID")]
        public ServerEntityKey StudyKey
        {
        get { return _studyKey; }
        set { _studyKey = value; }
        }
        #endregion

        #region Static Methods
        static public Series Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public Series Load(IPersistenceContext read, ServerEntityKey key)
        {
            ISeriesEntityBroker broker = read.GetBroker<ISeriesEntityBroker>();
            Series theObject = broker.Load(key);
            return theObject;
        }
        static public Series Insert(Series entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                Series newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public Series Insert(IUpdateContext update, Series entity)
        {
            ISeriesEntityBroker broker = update.GetBroker<ISeriesEntityBroker>();
            SeriesUpdateColumns updateColumns = new SeriesUpdateColumns();
            updateColumns.Modality = entity.Modality;
            updateColumns.NumberOfSeriesRelatedInstances = entity.NumberOfSeriesRelatedInstances;
            updateColumns.PerformedProcedureStepStartDate = entity.PerformedProcedureStepStartDate;
            updateColumns.PerformedProcedureStepStartTime = entity.PerformedProcedureStepStartTime;
            updateColumns.SeriesDescription = entity.SeriesDescription;
            updateColumns.SeriesInstanceUid = entity.SeriesInstanceUid;
            updateColumns.SeriesNumber = entity.SeriesNumber;
            updateColumns.ServerPartitionKey = entity.ServerPartitionKey;
            updateColumns.SourceApplicationEntityTitle = entity.SourceApplicationEntityTitle;
            updateColumns.StudyKey = entity.StudyKey;
            Series newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
