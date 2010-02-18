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

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;

    public partial class StudyDeleteRecordSelectCriteria : EntitySelectCriteria
    {
        public StudyDeleteRecordSelectCriteria()
        : base("StudyDeleteRecord")
        {}
        public StudyDeleteRecordSelectCriteria(StudyDeleteRecordSelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new StudyDeleteRecordSelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyInstanceUid")]
        public ISearchCondition<String> StudyInstanceUid
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyInstanceUid"))
              {
                 SubCriteria["StudyInstanceUid"] = new SearchCondition<String>("StudyInstanceUid");
              }
              return (ISearchCondition<String>)SubCriteria["StudyInstanceUid"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="Timestamp")]
        public ISearchCondition<DateTime> Timestamp
        {
            get
            {
              if (!SubCriteria.ContainsKey("Timestamp"))
              {
                 SubCriteria["Timestamp"] = new SearchCondition<DateTime>("Timestamp");
              }
              return (ISearchCondition<DateTime>)SubCriteria["Timestamp"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="ServerPartitionAE")]
        public ISearchCondition<String> ServerPartitionAE
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServerPartitionAE"))
              {
                 SubCriteria["ServerPartitionAE"] = new SearchCondition<String>("ServerPartitionAE");
              }
              return (ISearchCondition<String>)SubCriteria["ServerPartitionAE"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="FilesystemGUID")]
        public ISearchCondition<ServerEntityKey> FilesystemKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("FilesystemKey"))
              {
                 SubCriteria["FilesystemKey"] = new SearchCondition<ServerEntityKey>("FilesystemKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["FilesystemKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="BackupPath")]
        public ISearchCondition<String> BackupPath
        {
            get
            {
              if (!SubCriteria.ContainsKey("BackupPath"))
              {
                 SubCriteria["BackupPath"] = new SearchCondition<String>("BackupPath");
              }
              return (ISearchCondition<String>)SubCriteria["BackupPath"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="Reason")]
        public ISearchCondition<String> Reason
        {
            get
            {
              if (!SubCriteria.ContainsKey("Reason"))
              {
                 SubCriteria["Reason"] = new SearchCondition<String>("Reason");
              }
              return (ISearchCondition<String>)SubCriteria["Reason"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="AccessionNumber")]
        public ISearchCondition<String> AccessionNumber
        {
            get
            {
              if (!SubCriteria.ContainsKey("AccessionNumber"))
              {
                 SubCriteria["AccessionNumber"] = new SearchCondition<String>("AccessionNumber");
              }
              return (ISearchCondition<String>)SubCriteria["AccessionNumber"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="PatientId")]
        public ISearchCondition<String> PatientId
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientId"))
              {
                 SubCriteria["PatientId"] = new SearchCondition<String>("PatientId");
              }
              return (ISearchCondition<String>)SubCriteria["PatientId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="PatientsName")]
        public ISearchCondition<String> PatientsName
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientsName"))
              {
                 SubCriteria["PatientsName"] = new SearchCondition<String>("PatientsName");
              }
              return (ISearchCondition<String>)SubCriteria["PatientsName"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyId")]
        public ISearchCondition<String> StudyId
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyId"))
              {
                 SubCriteria["StudyId"] = new SearchCondition<String>("StudyId");
              }
              return (ISearchCondition<String>)SubCriteria["StudyId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyDescription")]
        public ISearchCondition<String> StudyDescription
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyDescription"))
              {
                 SubCriteria["StudyDescription"] = new SearchCondition<String>("StudyDescription");
              }
              return (ISearchCondition<String>)SubCriteria["StudyDescription"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyDate")]
        public ISearchCondition<String> StudyDate
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyDate"))
              {
                 SubCriteria["StudyDate"] = new SearchCondition<String>("StudyDate");
              }
              return (ISearchCondition<String>)SubCriteria["StudyDate"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyTime")]
        public ISearchCondition<String> StudyTime
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyTime"))
              {
                 SubCriteria["StudyTime"] = new SearchCondition<String>("StudyTime");
              }
              return (ISearchCondition<String>)SubCriteria["StudyTime"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="ArchiveInfo")]
        public ISearchCondition<XmlDocument> ArchiveInfo
        {
            get
            {
              if (!SubCriteria.ContainsKey("ArchiveInfo"))
              {
                 SubCriteria["ArchiveInfo"] = new SearchCondition<XmlDocument>("ArchiveInfo");
              }
              return (ISearchCondition<XmlDocument>)SubCriteria["ArchiveInfo"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="ExtendedInfo")]
        public ISearchCondition<String> ExtendedInfo
        {
            get
            {
              if (!SubCriteria.ContainsKey("ExtendedInfo"))
              {
                 SubCriteria["ExtendedInfo"] = new SearchCondition<String>("ExtendedInfo");
              }
              return (ISearchCondition<String>)SubCriteria["ExtendedInfo"];
            } 
        }
    }
}
