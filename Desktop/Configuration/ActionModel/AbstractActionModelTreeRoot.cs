#region License

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
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Trees;

namespace ClearCanvas.Desktop.Configuration.ActionModel
{
	public class AbstractActionModelTreeRoot : AbstractActionModelTreeBranch
	{
		internal event EventHandler<NodeValidationRequestedEventArgs> NodeValidationRequested;

		private readonly string _site;

		public AbstractActionModelTreeRoot(string site) : base(site)
		{
			_site = site;
		}

		public string Site
		{
			get { return _site; }
		}

		public ITree Tree
		{
			get { return base.Subtree; }
		}

		public ActionModelRoot GetAbstractActionModel()
		{
			ActionModelRoot actionModelRoot = new ActionModelRoot(_site);
			this.CreateActionModelRoot(actionModelRoot);
			return actionModelRoot;
		}

		internal override bool RequestValidation(AbstractActionModelTreeNode node, string propertyName, object value)
		{
			NodeValidationRequestedEventArgs e = new NodeValidationRequestedEventArgs(node, propertyName, value);
			EventsHelper.Fire(this.NodeValidationRequested, this, e);
			return e.IsValid;
		}
	}

	internal class NodeValidationRequestedEventArgs : EventArgs
	{
		public readonly AbstractActionModelTreeNode Node;
		public readonly string PropertyName;
		public readonly object Value;
		public bool IsValid { get; set; }

		public NodeValidationRequestedEventArgs(AbstractActionModelTreeNode node, string propertyName, object value)
		{
			this.Node = node;
			this.PropertyName = propertyName;
			this.Value = value;
			this.IsValid = true;
		}
	}
}