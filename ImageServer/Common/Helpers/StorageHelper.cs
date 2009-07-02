﻿#region License

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Common.Helpers
{
    
    public static class StorageHelper
    {
        /// <summary>
        /// Returns the name of the directory in the filesytem
        /// where the study with the specified information will be stored.
        /// </summary>
        /// <returns></returns>
        /// 
        public static string ResolveStorageFolder(
            ServerPartition partition, 
            string studyInstanceUid, 
            string studyDate,
            IPersistenceContext persistenceContext,
            bool checkExisting)
        {
            string folder;

            if (checkExisting)
            {
                StudyStorage storage = StudyHelper.FindStorage(persistenceContext, studyInstanceUid, partition);
                if (storage != null)
                {
                    folder = ImageServerCommonConfiguration.UseReceiveDateAsStudyFolder
                                    ? storage.InsertTime.ToString("yyyyMMdd")
                                    : String.IsNullOrEmpty(studyDate)
                                          ? ImageServerCommonConfiguration.DefaultStudyRootFolder
                                          : studyDate;
                    return folder;
                }
            }

            folder = ImageServerCommonConfiguration.UseReceiveDateAsStudyFolder
                                ? Platform.Time.ToString("yyyyMMdd")
                                : String.IsNullOrEmpty(studyDate)
                                      ? ImageServerCommonConfiguration.DefaultStudyRootFolder
                                      : studyDate;

            return folder;

        }


        /// <summary>
        /// Checks for a storage location for the study in the database, and creates a new location
        /// in the database if it doesn't exist.
        /// </summary>
        /// <param name="message">The DICOM message to create the storage location for.</param>
        /// <param name="partition">The partition where the study is being sent to</param>
        /// <returns>A <see cref="StudyStorageLocation"/> instance.</returns>
        static public StudyStorageLocation GetWritableStudyStorageLocation(DicomMessageBase message, ServerPartition partition)
        {
            String studyInstanceUid = message.DataSet[DicomTags.StudyInstanceUid].GetString(0, "");
            String studyDate = message.DataSet[DicomTags.StudyDate].GetString(0, "");

            FilesystemSelector selector = new FilesystemSelector(FilesystemMonitor.Instance);
            ServerFilesystemInfo filesystem = selector.SelectFilesystem(message);
            if (filesystem == null)
            {
                Platform.Log(LogLevel.Error, "Unable to select location for storing study.");

                return null;
            }


            using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                IQueryStudyStorageLocation locQuery = updateContext.GetBroker<IQueryStudyStorageLocation>();
                StudyStorageLocationQueryParameters locParms = new StudyStorageLocationQueryParameters();
                locParms.StudyInstanceUid = studyInstanceUid;
                locParms.ServerPartitionKey = partition.GetKey();
                IList<StudyStorageLocation> studyLocationList = locQuery.Find(locParms);

                if (studyLocationList.Count == 0)
                {
                    StudyStorage storage = StudyHelper.FindStorage(updateContext, studyInstanceUid, partition);

                    if (storage != null)
                    {
                        Platform.Log(LogLevel.Warn, "Study in {0} state.  Rejecting image.", storage.StudyStatusEnum.Description);
                        return null;
                    }

                    IInsertStudyStorage locInsert = updateContext.GetBroker<IInsertStudyStorage>();
                    InsertStudyStorageParameters insertParms = new InsertStudyStorageParameters();
                    insertParms.ServerPartitionKey = partition.GetKey();
                    insertParms.StudyInstanceUid = studyInstanceUid;
                    
                    insertParms.Folder = ResolveStorageFolder(partition, studyInstanceUid, studyDate, updateContext, false /* set to false for optimization because we are sure it's not in the system */);
                    insertParms.FilesystemKey = filesystem.Filesystem.GetKey();
                    insertParms.QueueStudyStateEnum = QueueStudyStateEnum.Idle;

                    if (message.TransferSyntax.LosslessCompressed)
                    {
                        insertParms.TransferSyntaxUid = message.TransferSyntax.UidString;
                        insertParms.StudyStatusEnum = StudyStatusEnum.OnlineLossless;
                    }
                    else if (message.TransferSyntax.LossyCompressed)
                    {
                        insertParms.TransferSyntaxUid = message.TransferSyntax.UidString;
                        insertParms.StudyStatusEnum = StudyStatusEnum.OnlineLossy;
                    }
                    else
                    {
                        insertParms.TransferSyntaxUid = TransferSyntax.ExplicitVrLittleEndianUid;
                        insertParms.StudyStatusEnum = StudyStatusEnum.Online;
                    }

                    studyLocationList = locInsert.Find(insertParms);

                    updateContext.Commit();
                }
                else
                {
                    if (!FilesystemMonitor.Instance.CheckFilesystemWriteable(studyLocationList[0].FilesystemKey))
                    {
                        Platform.Log(LogLevel.Warn, "Unable to find writable filesystem for study {0} on Partition {1}",
                                     studyInstanceUid, partition.Description);
                        return null;
                    }
                }

                //TODO:  Do we need to do something to identify a primary storage location?
                // Also, should the above check for writeable location check the other availab
                return studyLocationList[0];
            }
        }

    }

}
