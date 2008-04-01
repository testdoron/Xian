#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

using System.Threading;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Client.Adt
{
    [ExtensionPoint]
    public class RegistrationBookingWorkflowFolderExtensionPoint : ExtensionPoint<IFolder>
    {
    }

    [ExtensionPoint]
    public class RegistrationBookingWorkflowItemToolExtensionPoint : ExtensionPoint<ITool>
    {
    }

    [ExtensionPoint]
    public class RegistrationBookingWorkflowFolderToolExtensionPoint : ExtensionPoint<ITool>
    {
    }

    public class RegistrationBookingWorkflowFolderSystem : RegistrationWorkflowFolderSystemBase
    {
        public RegistrationBookingWorkflowFolderSystem(IFolderExplorerToolContext folderExplorer)
            : base(folderExplorer, 
            new RegistrationBookingWorkflowFolderExtensionPoint(),
            new RegistrationBookingWorkflowItemToolExtensionPoint(),
            new RegistrationBookingWorkflowFolderToolExtensionPoint())
        {
            if (Thread.CurrentPrincipal.IsInRole(AuthorityTokens.ViewUnfilteredWorkflowFolders))
            {
                this.AddFolder(new Folders.CompletedProtocolFolder(this));
                this.AddFolder(new Folders.SuspendedProtocolFolder(this));
                this.AddFolder(new Folders.RejectedProtocolFolder(this));
                this.AddFolder(new Folders.PendingProtocolFolder(this));
                this.AddFolder(new Folders.ToBeScheduledFolder(this));
            }
        }

        public override string DisplayName
        {
            get { return "Booking"; }
        }

        public override string PreviewUrl
        {
            get { return RegistrationPreviewComponentSettings.Default.BookingFolderSystemUrl; }
        }

        public override void SelectedItemDoubleClickedEventHandler(object sender, System.EventArgs e)
        {
            base.SelectedItemDoubleClickedEventHandler(sender, e);

            PatientBiographyTool biographyTool = (PatientBiographyTool)CollectionUtils.SelectFirst(this.ItemTools.Tools,
               delegate(ITool tool) { return tool is PatientBiographyTool; });

            if (biographyTool != null && biographyTool.Enabled)
                biographyTool.View();
        }
    }
}