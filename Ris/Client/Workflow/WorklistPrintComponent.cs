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

using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Desktop;

namespace ClearCanvas.Ris.Client.Workflow
{
	/// <summary>
	/// Extension point for views onto <see cref="WorklistPrintComponent"/>.
	/// </summary>
	[ExtensionPoint]
	public sealed class WorklistPrintComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	/// <summary>
	/// WorklistPrintComponent class.
	/// </summary>
	[AssociateView(typeof(WorklistPrintComponentViewExtensionPoint))]
	public class WorklistPrintComponent : ApplicationComponent
	{
		private WorklistPrintViewComponent _worklistPrintPreviewComponent;
		private ChildComponentHost _worklistPrintPreviewComponentHost;

		private readonly WorklistPrintViewComponent.PrintContext _printContext;

		public WorklistPrintComponent(string folderSystemName, string folderName, string folderDescription, int totalCount, List<object> items)
		{
			_printContext = new WorklistPrintViewComponent.PrintContext(folderSystemName, folderName, folderDescription, totalCount, items);
		}

		public override void Start()
		{
			_worklistPrintPreviewComponent = new WorklistPrintViewComponent(_printContext);
			_worklistPrintPreviewComponentHost = new ChildComponentHost(this.Host, _worklistPrintPreviewComponent);
			_worklistPrintPreviewComponentHost.StartComponent();

			base.Start();
		}

		public ApplicationComponentHost WorklistPrintPreviewComponentHost
		{
			get { return _worklistPrintPreviewComponentHost; }
		}

		public void Print()
		{
			if (DialogBoxAction.No == this.Host.DesktopWindow.ShowMessageBox(SR.MessagePrintWorklist, MessageBoxActions.YesNo))
				return;

			// print the rendered document
			_worklistPrintPreviewComponent.PrintDocument();

			this.Exit(ApplicationComponentExitCode.Accepted);
		}

		public void Close()
		{
			this.Exit(ApplicationComponentExitCode.None);
		}
	}
}
