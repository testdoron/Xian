#region License

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
using System.Drawing;
using System.Windows.Forms;
using ClearCanvas.ImageViewer.Utilities.StudyFilters.Tools.Actions;

namespace ClearCanvas.ImageViewer.Utilities.StudyFilters.View.WinForms.ToolStripFilterItems
{
	/// <summary>
	/// A custom <see cref="ToolStripItem"/> control that hosts a <see cref="ListFilterControl"/>.
	/// </summary>
	internal class ListFilterToolStripItem : ToolStripControlHost
	{
		private readonly ListFilterControl _filterList;
		private readonly ListFilterMenuAction _action;
		private readonly Panel _panel;
		private readonly Size _defaultSize;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="action">The action to which this view is bound.</param>
		public ListFilterToolStripItem(ListFilterMenuAction action)
			: base(new Panel())
		{
			const int idealPickerWidth = 150;
			const int idealPickerHeight = 300;

			_action = action;

			_filterList = new ListFilterControl(action.DataSource);
			_filterList.Dock = DockStyle.Fill;
			_filterList.BackColor = Color.Transparent;
			_filterList.Size = new Size(idealPickerWidth, idealPickerHeight);
			_filterList.ResetDropDownFocus += FilterList_ResetDropDownFocus;
			_panel = (Panel) base.Control;
			_panel.Size = _defaultSize = new Size(Math.Max(base.Width, idealPickerWidth), idealPickerHeight);
			_panel.Controls.Add(_filterList);

			base.AutoSize = false;
			base.BackColor = Color.Transparent;
			base.ControlAlign = ContentAlignment.TopCenter;
			base.Size = _defaultSize = new Size(Math.Max(base.Width, idealPickerWidth), idealPickerHeight);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_filterList.ResetDropDownFocus -= FilterList_ResetDropDownFocus;
			}
			base.Dispose(disposing);
		}

		protected override Size DefaultSize
		{
			get { return _defaultSize; }
		}

		public override Size GetPreferredSize(Size constrainingSize)
		{
			return _defaultSize;
		}

		protected override bool DismissWhenClicked
		{
			get { return false; }
		}

		private void FilterList_ResetDropDownFocus(object sender, EventArgs e)
		{
			if (base.Owner != null)
				base.Owner.Focus();
		}
	}
}