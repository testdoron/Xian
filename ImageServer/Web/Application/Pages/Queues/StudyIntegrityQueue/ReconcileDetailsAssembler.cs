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

using System.Collections.Generic;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.ImageServer.Core.Data;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Web.Common;
using ClearCanvas.ImageServer.Web.Common.Data.DataSource;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Queues.StudyIntegrityQueue
{
    public static class ReconcileDetailsAssembler
    {
        public static ReconcileDetails CreateReconcileDetails(StudyIntegrityQueueSummary item)
        {
            ReconcileDetails details = item.TheStudyIntegrityQueueItem.StudyIntegrityReasonEnum.Equals(
                                           StudyIntegrityReasonEnum.InconsistentData)
                                           ? new ReconcileDetails(item.TheStudyIntegrityQueueItem)
                                           : new DuplicateEntryDetails(item.TheStudyIntegrityQueueItem);

            Study study = item.StudySummary.TheStudy;
            details.StudyInstanceUid = study.StudyInstanceUid;

            //Set the demographic details of the Existing Patient
            details.ExistingStudy = new ReconcileDetails.StudyInfo();
            details.ExistingStudy.StudyInstanceUid = study.StudyInstanceUid;
            details.ExistingStudy.AccessionNumber = study.AccessionNumber;
            details.ExistingStudy.StudyDate = study.StudyDate;
            details.ExistingStudy.Patient.PatientID = study.PatientId;
            details.ExistingStudy.Patient.Name = study.PatientsName;
            details.ExistingStudy.Patient.Sex = study.PatientsSex;
            details.ExistingStudy.Patient.IssuerOfPatientID = study.IssuerOfPatientId;
            details.ExistingStudy.Patient.BirthDate = study.PatientsBirthDate;
            details.ExistingStudy.Series = CollectionUtils.Map(
                study.Series.Values,
                delegate(Series theSeries)
                    {
                        var seriesDetails = new ReconcileDetails.SeriesDetails
                                                {
                                                    Description = theSeries.SeriesDescription,
                                                    SeriesInstanceUid = theSeries.SeriesInstanceUid,
                                                    Modality = theSeries.Modality,
                                                    NumberOfInstances = theSeries.NumberOfSeriesRelatedInstances,
                                                    SeriesNumber = theSeries.SeriesNumber
                                                };
                        return seriesDetails;
                    });


            details.ConflictingImageSet = item.QueueData.Details;


            details.ConflictingStudyInfo = new ReconcileDetails.StudyInfo();

            if (item.QueueData.Details != null)
            {
                // extract the conflicting study info from Details
                details.ConflictingStudyInfo.AccessionNumber = item.QueueData.Details.StudyInfo.AccessionNumber;
                details.ConflictingStudyInfo.StudyDate = item.QueueData.Details.StudyInfo.StudyDate;
                details.ConflictingStudyInfo.StudyInstanceUid = item.QueueData.Details.StudyInfo.StudyInstanceUid;
                details.ConflictingStudyInfo.StudyDate = item.QueueData.Details.StudyInfo.StudyDate;

                details.ConflictingStudyInfo.Patient = new ReconcileDetails.PatientInfo
                                                           {
                                                               BirthDate =
                                                                   item.QueueData.Details.StudyInfo.PatientInfo.
                                                                   PatientsBirthdate,
                                                               IssuerOfPatientID =
                                                                   item.QueueData.Details.StudyInfo.PatientInfo.
                                                                   IssuerOfPatientId,
                                                               Name = item.QueueData.Details.StudyInfo.PatientInfo.Name,
                                                               PatientID =
                                                                   item.QueueData.Details.StudyInfo.PatientInfo.
                                                                   PatientId,
                                                               Sex = item.QueueData.Details.StudyInfo.PatientInfo.Sex
                                                           };

                details.ConflictingStudyInfo.Series =
                    CollectionUtils.Map(
                        item.QueueData.Details.StudyInfo.Series,
                        delegate(SeriesInformation input)
                            {
                                var seriesDetails = new ReconcileDetails.SeriesDetails
                                                        {
                                                            Description = input.SeriesDescription,
                                                            Modality = input.Modality,
                                                            SeriesInstanceUid = input.SeriesInstanceUid,
                                                            NumberOfInstances = input.NumberOfInstances
                                                        };
                                return seriesDetails;
                            });
            }
            else
            {
                // Extract the conflicting study info from StudyData
                // Note: Not all fields are available.
                ImageSetDescriptor desc =
                    ImageSetDescriptor.Parse(item.TheStudyIntegrityQueueItem.StudyData.DocumentElement);
                string value;

                if (desc.TryGetValue(DicomTags.AccessionNumber, out value))
                    details.ConflictingStudyInfo.AccessionNumber = value;

                if (desc.TryGetValue(DicomTags.StudyDate, out value))
                    details.ConflictingStudyInfo.StudyDate = value;

                if (desc.TryGetValue(DicomTags.StudyInstanceUid, out value))
                    details.ConflictingStudyInfo.StudyInstanceUid = value;

                details.ConflictingStudyInfo.Patient = new ReconcileDetails.PatientInfo();

                if (desc.TryGetValue(DicomTags.PatientsBirthDate, out value))
                    details.ConflictingStudyInfo.Patient.BirthDate = value;

                if (desc.TryGetValue(DicomTags.IssuerOfPatientId, out value))
                    details.ConflictingStudyInfo.Patient.IssuerOfPatientID = value;

                if (desc.TryGetValue(DicomTags.PatientsName, out value))
                    details.ConflictingStudyInfo.Patient.Name = value;

                if (desc.TryGetValue(DicomTags.PatientId, out value))
                    details.ConflictingStudyInfo.Patient.PatientID = value;

                if (desc.TryGetValue(DicomTags.PatientsSex, out value))
                    details.ConflictingStudyInfo.Patient.Sex = value;


                var series = new List<ReconcileDetails.SeriesDetails>();
                details.ConflictingStudyInfo.Series = series;

                var uidBroker =
                    HttpContextData.Current.ReadContext.GetBroker<IStudyIntegrityQueueUidEntityBroker>();
                var criteria = new StudyIntegrityQueueUidSelectCriteria();
                criteria.StudyIntegrityQueueKey.EqualTo(item.TheStudyIntegrityQueueItem.GetKey());

                IList<StudyIntegrityQueueUid> uids = uidBroker.Find(criteria);

                Dictionary<string, List<StudyIntegrityQueueUid>> seriesGroups = CollectionUtils.GroupBy(uids,
                                                                                                        uid =>
                                                                                                        uid.
                                                                                                            SeriesInstanceUid);

                foreach (string seriesUid in seriesGroups.Keys)
                {
                    var seriesDetails = new ReconcileDetails.SeriesDetails
                                            {
                                                SeriesInstanceUid = seriesUid,
                                                Description = seriesGroups[seriesUid][0].SeriesDescription,
                                                NumberOfInstances = seriesGroups[seriesUid].Count
                                            };
                    //seriesDetails.Modality = "N/A";
                    series.Add(seriesDetails);
                }
            }


            return details;
        }
    }
}