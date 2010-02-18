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

    public partial class ServerPartitionSelectCriteria : EntitySelectCriteria
    {
        public ServerPartitionSelectCriteria()
        : base("ServerPartition")
        {}
        public ServerPartitionSelectCriteria(ServerPartitionSelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new ServerPartitionSelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="Enabled")]
        public ISearchCondition<Boolean> Enabled
        {
            get
            {
              if (!SubCriteria.ContainsKey("Enabled"))
              {
                 SubCriteria["Enabled"] = new SearchCondition<Boolean>("Enabled");
              }
              return (ISearchCondition<Boolean>)SubCriteria["Enabled"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="Description")]
        public ISearchCondition<String> Description
        {
            get
            {
              if (!SubCriteria.ContainsKey("Description"))
              {
                 SubCriteria["Description"] = new SearchCondition<String>("Description");
              }
              return (ISearchCondition<String>)SubCriteria["Description"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AeTitle")]
        public ISearchCondition<String> AeTitle
        {
            get
            {
              if (!SubCriteria.ContainsKey("AeTitle"))
              {
                 SubCriteria["AeTitle"] = new SearchCondition<String>("AeTitle");
              }
              return (ISearchCondition<String>)SubCriteria["AeTitle"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="Port")]
        public ISearchCondition<Int32> Port
        {
            get
            {
              if (!SubCriteria.ContainsKey("Port"))
              {
                 SubCriteria["Port"] = new SearchCondition<Int32>("Port");
              }
              return (ISearchCondition<Int32>)SubCriteria["Port"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="PartitionFolder")]
        public ISearchCondition<String> PartitionFolder
        {
            get
            {
              if (!SubCriteria.ContainsKey("PartitionFolder"))
              {
                 SubCriteria["PartitionFolder"] = new SearchCondition<String>("PartitionFolder");
              }
              return (ISearchCondition<String>)SubCriteria["PartitionFolder"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AcceptAnyDevice")]
        public ISearchCondition<Boolean> AcceptAnyDevice
        {
            get
            {
              if (!SubCriteria.ContainsKey("AcceptAnyDevice"))
              {
                 SubCriteria["AcceptAnyDevice"] = new SearchCondition<Boolean>("AcceptAnyDevice");
              }
              return (ISearchCondition<Boolean>)SubCriteria["AcceptAnyDevice"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AuditDeleteStudy")]
        public ISearchCondition<Boolean> AuditDeleteStudy
        {
            get
            {
              if (!SubCriteria.ContainsKey("AuditDeleteStudy"))
              {
                 SubCriteria["AuditDeleteStudy"] = new SearchCondition<Boolean>("AuditDeleteStudy");
              }
              return (ISearchCondition<Boolean>)SubCriteria["AuditDeleteStudy"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="AutoInsertDevice")]
        public ISearchCondition<Boolean> AutoInsertDevice
        {
            get
            {
              if (!SubCriteria.ContainsKey("AutoInsertDevice"))
              {
                 SubCriteria["AutoInsertDevice"] = new SearchCondition<Boolean>("AutoInsertDevice");
              }
              return (ISearchCondition<Boolean>)SubCriteria["AutoInsertDevice"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="DefaultRemotePort")]
        public ISearchCondition<Int32> DefaultRemotePort
        {
            get
            {
              if (!SubCriteria.ContainsKey("DefaultRemotePort"))
              {
                 SubCriteria["DefaultRemotePort"] = new SearchCondition<Int32>("DefaultRemotePort");
              }
              return (ISearchCondition<Int32>)SubCriteria["DefaultRemotePort"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="StudyCount")]
        public ISearchCondition<Int32> StudyCount
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyCount"))
              {
                 SubCriteria["StudyCount"] = new SearchCondition<Int32>("StudyCount");
              }
              return (ISearchCondition<Int32>)SubCriteria["StudyCount"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="DuplicateSopPolicyEnum")]
        public ISearchCondition<DuplicateSopPolicyEnum> DuplicateSopPolicyEnum
        {
            get
            {
              if (!SubCriteria.ContainsKey("DuplicateSopPolicyEnum"))
              {
                 SubCriteria["DuplicateSopPolicyEnum"] = new SearchCondition<DuplicateSopPolicyEnum>("DuplicateSopPolicyEnum");
              }
              return (ISearchCondition<DuplicateSopPolicyEnum>)SubCriteria["DuplicateSopPolicyEnum"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchAccessionNumber")]
        public ISearchCondition<Boolean> MatchAccessionNumber
        {
            get
            {
              if (!SubCriteria.ContainsKey("MatchAccessionNumber"))
              {
                 SubCriteria["MatchAccessionNumber"] = new SearchCondition<Boolean>("MatchAccessionNumber");
              }
              return (ISearchCondition<Boolean>)SubCriteria["MatchAccessionNumber"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchIssuerOfPatientId")]
        public ISearchCondition<Boolean> MatchIssuerOfPatientId
        {
            get
            {
              if (!SubCriteria.ContainsKey("MatchIssuerOfPatientId"))
              {
                 SubCriteria["MatchIssuerOfPatientId"] = new SearchCondition<Boolean>("MatchIssuerOfPatientId");
              }
              return (ISearchCondition<Boolean>)SubCriteria["MatchIssuerOfPatientId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientId")]
        public ISearchCondition<Boolean> MatchPatientId
        {
            get
            {
              if (!SubCriteria.ContainsKey("MatchPatientId"))
              {
                 SubCriteria["MatchPatientId"] = new SearchCondition<Boolean>("MatchPatientId");
              }
              return (ISearchCondition<Boolean>)SubCriteria["MatchPatientId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientsBirthDate")]
        public ISearchCondition<Boolean> MatchPatientsBirthDate
        {
            get
            {
              if (!SubCriteria.ContainsKey("MatchPatientsBirthDate"))
              {
                 SubCriteria["MatchPatientsBirthDate"] = new SearchCondition<Boolean>("MatchPatientsBirthDate");
              }
              return (ISearchCondition<Boolean>)SubCriteria["MatchPatientsBirthDate"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientsName")]
        public ISearchCondition<Boolean> MatchPatientsName
        {
            get
            {
              if (!SubCriteria.ContainsKey("MatchPatientsName"))
              {
                 SubCriteria["MatchPatientsName"] = new SearchCondition<Boolean>("MatchPatientsName");
              }
              return (ISearchCondition<Boolean>)SubCriteria["MatchPatientsName"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerPartition", ColumnName="MatchPatientsSex")]
        public ISearchCondition<Boolean> MatchPatientsSex
        {
            get
            {
              if (!SubCriteria.ContainsKey("MatchPatientsSex"))
              {
                 SubCriteria["MatchPatientsSex"] = new SearchCondition<Boolean>("MatchPatientsSex");
              }
              return (ISearchCondition<Boolean>)SubCriteria["MatchPatientsSex"];
            } 
        }
    }
}
