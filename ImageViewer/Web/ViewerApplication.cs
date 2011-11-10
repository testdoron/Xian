#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using ClearCanvas.Common;
using ClearCanvas.Common.Configuration;
using ClearCanvas.Desktop;
using ClearCanvas.Dicom.ServiceModel.Query;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Web.Common.Messages;
using ClearCanvas.Web.Common;
using ClearCanvas.Web.Common.Events;
using ClearCanvas.Web.Services;
using ClearCanvas.ImageViewer.Web.Common;
using ClearCanvas.ImageViewer.Web.EntityHandlers;
using ClearCanvas.ImageViewer.Web.Common.Entities;
using Application=ClearCanvas.Desktop.Application;
using ClearCanvas.Common.Utilities;
using Message=ClearCanvas.Web.Common.Message;

namespace ClearCanvas.ImageViewer.Web
{
	//TODO (CR May 2010): resource strings.

	[ExtensionOf(typeof(ExceptionTranslatorExtensionPoint))]
	internal class ExceptionTranslator : IExceptionTranslator
	{
		#region IExceptionTranslator Members

		public string Translate(Exception e)
		{
			//TODO (CR April 2011): Figure out how to share the Exception Policies for these messages ...
			//Current ExceptionHandler/Policy design just doesn't work for this at all.
			if (e.GetType().Equals(typeof(InUseLoadStudyException)))
				return ImageViewer.SR.MessageLoadStudyFailedInUse;
			if (e.GetType().Equals(typeof(NearlineLoadStudyException)))
			{
				return ((NearlineLoadStudyException)e).IsStudyBeingRestored
                    ? ImageViewer.SR.MessageLoadStudyFailedNearline : ImageViewer.SR.MessageLoadStudyFailedNearlineNoRestore;
			}
			if (e.GetType().Equals(typeof(OfflineLoadStudyException)))
                return ImageViewer.SR.MessageLoadStudyFailedOffline;
			if (e.GetType().Equals(typeof(NotFoundLoadStudyException)))
                return ImageViewer.SR.MessageLoadStudyFailedNotFound;
			if (e.GetType().Equals(typeof(LoadStudyException)))
				return SR.MessageStudyCouldNotBeLoaded;
			if (e is LoadMultipleStudiesException)
				return ((LoadMultipleStudiesException)e).GetUserMessage();

			if (e.GetType().Equals(typeof(NoVisibleDisplaySetsException)))
				return ImageViewer.SR.MessageNoVisibleDisplaySets;

			if (e.GetType().Equals(typeof(PatientStudiesNotFoundException)))
				return SR.MessagePatientStudiesNotFound;
			if (e.GetType().Equals(typeof(AccessionStudiesNotFoundException)))
				return SR.MessageAccessionStudiesNotFound;
			if (e.GetType().Equals(typeof(InvalidRequestException)))
				return e.Message;
			return null;
		}

		#endregion
	}

	internal class PatientStudiesNotFoundException : Exception
	{
		public PatientStudiesNotFoundException(string patientId)
			:base(String.Format("Studies for patient '{0}' could not be found.", patientId))
		{
		}
	}

	internal class AccessionStudiesNotFoundException : Exception
	{
		public AccessionStudiesNotFoundException(string accession)
			: base(String.Format("Studies matching accession '{0}' could not be found.", accession))
		{
		}
	}

	internal class InvalidRequestException : Exception
	{
		public InvalidRequestException()
			: base("The request must contain at least one valid study instance uid, patient id, or accession#.")
		{
		}
	}

    internal class RemoteClientInformation
    {
        public string IPAddress {get;set;}
    }

	[Application(typeof(StartViewerApplicationRequest))]
	[ExtensionOf(typeof(ApplicationExtensionPoint))]
	public class ViewerApplication : ClearCanvas.Web.Services.Application
	{
        private static readonly object _syncLock = new object();
		private Common.ViewerApplication _app;
		private ImageViewerComponent _viewer;
		private EntityHandler _viewerHandler;

	    private readonly RemoteClientInformation _client;
        
        public  ViewerApplication()
        {
            _client = new RemoteClientInformation
                          {
                              IPAddress = GetClientAddress(OperationContext.Current)
                          };

        }

        public override string InstanceName
        {
            get
            {
                return String.Format("WebStation (user={0}, ip={1})",
                                         Principal != null ? Principal.Identity.Name : "Unknown",
                                         _client.IPAddress);   

            }
        }

		private static IList<StudyRootStudyIdentifier> FindStudies(StartViewerApplicationRequest request)
		{
			bool invalidRequest = true;
			List<StudyRootStudyIdentifier> results = new List<StudyRootStudyIdentifier>();

			using (StudyRootQueryBridge bridge = new StudyRootQueryBridge(Platform.GetService<IStudyRootQuery>()))
			{
				if (request.StudyInstanceUid != null && request.StudyInstanceUid.Length > 0)
				{
					foreach (string studyUid in request.StudyInstanceUid)
					{
						//TODO (CR May 2010): can actually trigger a query of all studies
						if (!String.IsNullOrEmpty(studyUid))
							invalidRequest = false;

                        //TODO (CR May 2010): if request.AeTitle is set, assign RetrieveAeTitle parameter in 
                        // StudyRootStudyIndentifer to this value.  Update the query code to then only
                        // search this specified partition and remove the loop code below that looks
                        // for matching AeTitles.
						StudyRootStudyIdentifier identifier = new StudyRootStudyIdentifier
						                                          {
						                                              StudyInstanceUid = studyUid
						                                          };
					    IList<StudyRootStudyIdentifier> studies = bridge.StudyQuery(identifier);

					    bool found = false;
						foreach (StudyRootStudyIdentifier study in studies)
						{
						    if (!string.IsNullOrEmpty(request.AeTitle) && !study.RetrieveAeTitle.Equals(request.AeTitle)) continue;

						    results.Add(study);
						    found = true;
						}

                        if (!found)
                            throw new NotFoundLoadStudyException(studyUid);
                    }
				}
				if (request.PatientId != null && request.PatientId.Length > 0)
				{
					foreach (string patientId in request.PatientId)
					{
						if (!String.IsNullOrEmpty(patientId))
							invalidRequest = false;

						StudyRootStudyIdentifier identifier = new StudyRootStudyIdentifier
						                                          {
						                                              PatientId = patientId
						                                          };

					    IList<StudyRootStudyIdentifier> studies = bridge.StudyQuery(identifier);
					    bool found = false;
						foreach (StudyRootStudyIdentifier study in studies)
						{
						    if (!string.IsNullOrEmpty(request.AeTitle) && !study.RetrieveAeTitle.Equals(request.AeTitle)) continue;

						    results.Add(study);
						    found = true;
						}

                        if (!found)
                            throw new PatientStudiesNotFoundException(patientId);
                    }
				}
				if (request.AccessionNumber != null && request.AccessionNumber.Length > 0)
				{
					foreach (string accession in request.AccessionNumber)
					{
						if (!String.IsNullOrEmpty(accession))
							invalidRequest = false;

						StudyRootStudyIdentifier identifier = new StudyRootStudyIdentifier
						                                          {
						                                              AccessionNumber = accession
						                                          };

					    IList<StudyRootStudyIdentifier> studies = bridge.StudyQuery(identifier);

					    bool found = false;
						foreach (StudyRootStudyIdentifier study in studies)
						{
						    if (!string.IsNullOrEmpty(request.AeTitle) && !study.RetrieveAeTitle.Equals(request.AeTitle)) continue;

						    results.Add(study);
						    found = true;
						}

                        if (!found)
                            throw new AccessionStudiesNotFoundException(accession);
                    }
				}
			}

			if (invalidRequest)
				throw new InvalidRequestException();

			return results;
		}

		private static bool AnySopsLoaded(IImageViewer imageViewer)
		{
			foreach (Patient patient in imageViewer.StudyTree.Patients)
			{
				foreach (Study study in patient.Studies)
				{
					foreach (Series series in study.Series)
					{
						foreach (Sop sop in series.Sops)
						{
							return true;
						}
					}
				}
			}

			return false;
		}


	    protected override EventSet OnGetPendingOutboundEvent(int wait)
	    {
            if (_context == null)
            {
                string reason = string.Format("Application context no longer exists");
                throw new Exception(reason);
            }

            return _context.GetPendingOutboundEvent(wait);
	    }


	    protected override ProcessMessagesResult OnProcessMessageEnd(MessageSet messageSet, bool messageWasProcessed)
	    {
            if (!messageWasProcessed)
            {
                return new ProcessMessagesResult { EventSet = null, Pending = true };
            }
            
            bool hasMouseMoveMsg = false;
            foreach (Message m in messageSet.Messages)
            {
                if (m is MouseMoveMessage)
                {
                    hasMouseMoveMsg = true;
                    break;
                }
            }
            EventSet ev = GetPendingOutboundEvent(hasMouseMoveMsg ? 100 : 0);

            return new ProcessMessagesResult { EventSet = ev, Pending = false };
	    }

	    protected override void OnStart(StartApplicationRequest request)
		{
			lock (_syncLock)
			{
                Platform.Log(LogLevel.Info, "Viewer Application is starting...");
                if (Application.Instance == null)
					Platform.StartApp();
			}


            if (Platform.IsLogLevelEnabled(LogLevel.Debug))
                Platform.Log(LogLevel.Debug, "Finding studies...");
			var startRequest = (StartViewerApplicationRequest)request;
			IList<StudyRootStudyIdentifier> studies = FindStudies(startRequest);

			List<LoadStudyArgs> loadArgs = CollectionUtils.Map(studies, (StudyRootStudyIdentifier identifier) => CreateLoadStudyArgs(identifier));

		    DesktopWindowCreationArgs args = new DesktopWindowCreationArgs("", Identifier.ToString());
            WebDesktopWindow window = new WebDesktopWindow(args, Application.Instance);
            window.Open();

            _viewer = CreateViewerComponent(startRequest);

			try
			{
                if (Platform.IsLogLevelEnabled(LogLevel.Debug))
                    Platform.Log(LogLevel.Debug, "Loading study...");
                _viewer.LoadStudies(loadArgs);
			}
			catch (Exception e)
			{
				if (!AnySopsLoaded(_viewer)) //end the app.
                    throw;

				//Show an error and continue.
				ExceptionHandler.Report(e, window);
			}

            if (Platform.IsLogLevelEnabled(LogLevel.Debug))
                Platform.Log(LogLevel.Info, "Launching viewer...");
			
			ImageViewerComponent.Launch(_viewer, window, "");

			_viewerHandler = EntityHandler.Create<ViewerEntityHandler>();
			_viewerHandler.SetModelObject(_viewer);
		    _app = new Common.ViewerApplication
		               {
		                   Identifier = Identifier,
		                   Viewer = (Viewer) _viewerHandler.GetEntity(),

                           VersionString = String.Format("{0} [{1}]", 
                                ProductInformation.GetNameAndVersion(false, true),
                                String.Format("{0}.{1}.{2}.{3}", 
                                    ProductInformation.Version.Major, ProductInformation.Version.Minor, 
                                    ProductInformation.Version.Build, ProductInformation.Version.Revision))
			           };
            

            // Push the ViewerApplication object to the client
            Event @event = new PropertyChangedEvent
            {
                PropertyName = "Application",
                Value = _app,
                Identifier = Guid.NewGuid(),
                SenderId = request.Identifier
            };

            ApplicationContext.Current.FireEvent(@event);
		}


	    public static LoadStudyArgs CreateLoadStudyArgs(StudyRootStudyIdentifier identifier)
	    {

            // TODO: Need to think about this more. What's the best way to swap different loader?
            // Do we need to support loading studies from multiple servers? 

            if (WebViewerServices.Default.StudyLoaderName.Equals("CC_WEBSTATION_STREAMING"))
	        {
	            string host = WebViewerServices.Default.ArchiveServerHostname;
	            int port = WebViewerServices.Default.ArchiveServerPort;

	            int headerPort = WebViewerServices.Default.ArchiveServerHeaderPort;
	            int wadoPort = WebViewerServices.Default.ArchiveServerWADOPort;

	            var serverAe = new StudyManagement.ApplicationEntity(host,
	                                                                 identifier.RetrieveAeTitle,
	                                                                 identifier.RetrieveAeTitle, port, true,
	                                                                 headerPort, wadoPort);

	            return new LoadStudyArgs(identifier.StudyInstanceUid, serverAe, WebViewerServices.Default.StudyLoaderName);
	        }
	        else
	        {
	            throw new NotSupportedException("Only streaming study loader is supported at this time");
	        }
	    }


        private ImageViewerComponent CreateViewerComponent(StartViewerApplicationRequest request)
        {
            if (request.LoadStudyOptions & LoadStudyOptions.KeyImagesOnly == LoadStudyOptions.KeyImagesOnly)
                return new ImageViewerComponent(new KeyImageLayoutManager());
            else
                return new ImageViewerComponent(LayoutManagerCreationParameters.Extended);
        }

	    protected override void OnStop()
		{
			if (_viewerHandler != null)
			{
				_viewerHandler.Dispose();
				_viewerHandler = null;
			}

			//TODO (CR May 2010): WebDesktopWindow shouldn't hang around, but we can check.
			if (_viewer != null)
			{
				_viewer.Stop();
				_viewer.Dispose();
				_viewer = null;
			}
		}

		protected override ClearCanvas.Web.Common.Application GetContractObject()
		{
			return _app;
		}


        private static string GetClientAddress(OperationContext context)
        {
            if (context == null)
                return "Unknonw";

            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            return endpoint != null ? endpoint.Address : "Unknown";
        }
        
	}

    [ExtensionOf(typeof(SettingsStoreExtensionPoint))]
    public class StandardSettingsProvider : ISettingsStore
    {
		public bool IsOnline
		{
			get { return true; }
		}
		
		public bool SupportsImport
        {
            get { return false; }
        }

        public IList<SettingsGroupDescriptor> ListSettingsGroups()
        {
            return new List<SettingsGroupDescriptor>();
        }

        public SettingsGroupDescriptor GetPreviousSettingsGroup(SettingsGroupDescriptor group)
        {
            return null;
        }

        public IList<SettingsPropertyDescriptor> ListSettingsProperties(SettingsGroupDescriptor group)
        {
            return new List<SettingsPropertyDescriptor>();
        }

        public void ImportSettingsGroup(SettingsGroupDescriptor group, List<SettingsPropertyDescriptor> properties)
        {
            throw new NotSupportedException();
        }

        public Dictionary<string, string> GetSettingsValues(SettingsGroupDescriptor group, string user, string instanceKey)
        {
            return new Dictionary<string, string>();
        }

        public void PutSettingsValues(SettingsGroupDescriptor group, string user, string instanceKey, Dictionary<string, string> dirtyValues)
        {

        }

        public void RemoveUserSettings(SettingsGroupDescriptor group, string user, string instanceKey)
        {
            //throw new NotSupportedException();
        }
    }
}
