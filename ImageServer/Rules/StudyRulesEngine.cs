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

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities.Xml;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Rules
{
	/// <summary>
	/// A simplified view of the rules engine to apply study level and Series level rules on a study in a given Storage Location
	/// </summary>
	public class StudyRulesEngine
	{
		#region Private Members
		private readonly StudyStorageLocation _location;
		private ServerRulesEngine _seriesRulesEngine;
		private ServerRulesEngine _studyRulesEngine;
		private readonly ServerPartition _partition;
		private StudyXml _studyXml;
		#endregion

		#region Constructors
		public StudyRulesEngine(StudyStorageLocation location, ServerPartition partition)
		{
			_location = location;
			_partition = partition;
			if (_partition == null)
				_partition = ServerPartition.Load(_location.ServerPartitionKey);
		}
		public StudyRulesEngine(StudyStorageLocation location, ServerPartition partition, StudyXml studyXml)
		{
			_studyXml = studyXml;
			_location = location;
			_partition = partition;
			if (_partition == null)
				_partition = ServerPartition.Load(_location.ServerPartitionKey);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Apply the Rules engine.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method applies the rules engine to the first image in each series within a study.
		/// The assumption is that the actions generated by the engine can handle being applied more
		/// than once for the same study.  This is also done to handle the case of multi-modality
		/// studies where you may want the rules to be run against each series, because they may 
		/// apply differently.  
		/// </para>
		/// <para>
		/// Note that we are still applying series level moves, although there currently are not
		/// any series level rules.  We've somewhat turned the study level rules into series
		/// level rules.
		/// </para>
		/// </remarks>
		public void Apply(ServerRuleApplyTimeEnum applyTime)
		{

			using(ServerCommandProcessor theProcessor = new ServerCommandProcessor("Study Rule Processor"))
			{
                Apply(applyTime, theProcessor);

                if (false == theProcessor.Execute())
                {
                    Platform.Log(LogLevel.Error,
                                 "Unexpected failure processing Study level rules for study {0} on partition {1} for {2} apply time",
                                 _location.StudyInstanceUid, _partition.Description, applyTime.Description);
                }
			}

			
		}

		public void Apply(ServerRuleApplyTimeEnum applyTime, ServerCommandProcessor theProcessor)
		{
			_studyRulesEngine = new ServerRulesEngine(applyTime, _location.ServerPartitionKey);
			_studyRulesEngine.Load();

			List<string> files = GetFirstInstanceInEachStudySeries();
			if (files.Count == 0)
			{
				string message =
					String.Format("Unexectedly unable to find SOP instances for rules engine in each series in study: {0}",
					              _location.StudyInstanceUid);
				Platform.Log(LogLevel.Error, message);
				throw new ApplicationException(message);
			}

			Platform.Log(LogLevel.Info, "Processing Study Level rules for study {0} on partition {1} at {2} apply time",
			             _location.StudyInstanceUid, _partition.Description, applyTime.Description);

			foreach (string seriesFilePath in files)
			{
				DicomFile theFile = new DicomFile(seriesFilePath);
				theFile.Load(DicomReadOptions.Default);
				ServerActionContext context =
					new ServerActionContext(theFile, _location.FilesystemKey, _partition, _location.Key);
				context.CommandProcessor = theProcessor;
				_studyRulesEngine.Execute(context);

				ProcessSeriesRules(theFile, theProcessor);
			}

			if (applyTime.Equals(ServerRuleApplyTimeEnum.StudyProcessed))
			{
				// This is a bit kludgy, but we had a problem with studies with only 1 image incorectlly
				// having archive requests inserted when they were scheduled for deletion.  Calling
				// this command here so that if a delete is inserted at the study level, we will remove
				// the previously inserted archive request for the study.  Note also this has to be done
				// after the rules engine is executed.
				theProcessor.AddCommand(new InsertArchiveQueueCommand(_location.ServerPartitionKey, _location.Key));
			}
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Get a list of paths to the first image in each series within the study being processed.
		/// </summary>
		/// <returns></returns>
		private List<string> GetFirstInstanceInEachStudySeries()
		{
			List<string> fileList = new List<string>();

			if (_studyXml == null)
			{
			    string studyXml = _location.GetStudyXmlPath();

				if (!File.Exists(studyXml))
				{
					return fileList;
				}

				_studyXml = new StudyXml();

				using (FileStream stream = FileStreamOpener.OpenForRead(studyXml, FileMode.Open))
				{
					XmlDocument theDoc = new XmlDocument();
					StudyXmlIo.Read(theDoc, stream);
					stream.Close();
					_studyXml.SetMemento(theDoc);
				}
			}

			// Note, we try and force ourselves to have an uncompressed 
			// image, if one exists.  That way the rules will be reapplied on the object
			// if necessary for compression.
			foreach (SeriesXml seriesXml in _studyXml)
			{
				InstanceXml saveInstance = null;

				foreach (InstanceXml instance in seriesXml)
				{
					if (instance.TransferSyntax.Encapsulated)
					{
						if (saveInstance == null)
							saveInstance = instance;
					}
					else
					{
						saveInstance = instance;
						break;
					}
				}

				if (saveInstance != null)
				{
					string path = Path.Combine(_location.GetStudyPath(), seriesXml.SeriesInstanceUid);
					path = Path.Combine(path, saveInstance.SopInstanceUid + ServerPlatform.DicomFileExtension);
					fileList.Add(path);
				}
			}

			return fileList;
		}

		/// <summary>
		/// Method for applying rules when a new series has been inserted.
		/// </summary>
		/// <param name="file">The DICOM file being processed.</param>
		/// <param name="processor">The command processor</param>
		private void ProcessSeriesRules(DicomFile file, ServerCommandProcessor processor)
		{
			if (_seriesRulesEngine == null)
			{
				_seriesRulesEngine = new ServerRulesEngine(ServerRuleApplyTimeEnum.SeriesProcessed, _location.ServerPartitionKey);
				_seriesRulesEngine.Load();
			}
			else
			{
				_seriesRulesEngine.Statistics.LoadTime.Reset();
				_seriesRulesEngine.Statistics.ExecutionTime.Reset();
			}

			ServerActionContext context = new ServerActionContext(file, _location.FilesystemKey, _partition, _location.Key);

			context.CommandProcessor = processor;

			_seriesRulesEngine.Execute(context);

		}
		#endregion
	}
}
