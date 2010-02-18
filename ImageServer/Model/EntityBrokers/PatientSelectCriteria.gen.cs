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

    public partial class PatientSelectCriteria : EntitySelectCriteria
    {
        public PatientSelectCriteria()
        : base("Patient")
        {}
        public PatientSelectCriteria(PatientSelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new PatientSelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="ServerPartitionGUID")]
        public ISearchCondition<ServerEntityKey> ServerPartitionKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServerPartitionKey"))
              {
                 SubCriteria["ServerPartitionKey"] = new SearchCondition<ServerEntityKey>("ServerPartitionKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["ServerPartitionKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="NumberOfPatientRelatedStudies")]
        public ISearchCondition<Int32> NumberOfPatientRelatedStudies
        {
            get
            {
              if (!SubCriteria.ContainsKey("NumberOfPatientRelatedStudies"))
              {
                 SubCriteria["NumberOfPatientRelatedStudies"] = new SearchCondition<Int32>("NumberOfPatientRelatedStudies");
              }
              return (ISearchCondition<Int32>)SubCriteria["NumberOfPatientRelatedStudies"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="NumberOfPatientRelatedSeries")]
        public ISearchCondition<Int32> NumberOfPatientRelatedSeries
        {
            get
            {
              if (!SubCriteria.ContainsKey("NumberOfPatientRelatedSeries"))
              {
                 SubCriteria["NumberOfPatientRelatedSeries"] = new SearchCondition<Int32>("NumberOfPatientRelatedSeries");
              }
              return (ISearchCondition<Int32>)SubCriteria["NumberOfPatientRelatedSeries"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="NumberOfPatientRelatedInstances")]
        public ISearchCondition<Int32> NumberOfPatientRelatedInstances
        {
            get
            {
              if (!SubCriteria.ContainsKey("NumberOfPatientRelatedInstances"))
              {
                 SubCriteria["NumberOfPatientRelatedInstances"] = new SearchCondition<Int32>("NumberOfPatientRelatedInstances");
              }
              return (ISearchCondition<Int32>)SubCriteria["NumberOfPatientRelatedInstances"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="SpecificCharacterSet")]
        public ISearchCondition<String> SpecificCharacterSet
        {
            get
            {
              if (!SubCriteria.ContainsKey("SpecificCharacterSet"))
              {
                 SubCriteria["SpecificCharacterSet"] = new SearchCondition<String>("SpecificCharacterSet");
              }
              return (ISearchCondition<String>)SubCriteria["SpecificCharacterSet"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="PatientsName")]
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
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="PatientId")]
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
        [EntityFieldDatabaseMappingAttribute(TableName="Patient", ColumnName="IssuerOfPatientId")]
        public ISearchCondition<String> IssuerOfPatientId
        {
            get
            {
              if (!SubCriteria.ContainsKey("IssuerOfPatientId"))
              {
                 SubCriteria["IssuerOfPatientId"] = new SearchCondition<String>("IssuerOfPatientId");
              }
              return (ISearchCondition<String>)SubCriteria["IssuerOfPatientId"];
            } 
        }
    }
}
