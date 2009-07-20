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

#region DropDownListFilterHeaderCell License

// Portions of this code have been taken from MSDN article aa480727:
// "Building a Drop-Down Filter List for a DataGridView Column Header Cell."
// The article is available at http://msdn.microsoft.com/en-us/library/aa480727.aspx
// The following is the license notice on the original source code.

//---------------------------------------------------------------------
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.ImageViewer.Utilities.StudyFilters.View.WinForms
{
	public class StudyFilterColumnHeaderCell : DataGridViewColumnHeaderCell
	{
		private event EventHandler _filteredChanged;
		private readonly ChangeNotifier<DataGridView> _dataGridViewChangeNotifier;

		private Rectangle _dropDownButtonBounds = Rectangle.Empty;
		private int _dropDownButtonPaddingOffset = 0;
		private bool _filtered = false;
		private bool _dropDownEnabled = true;
		private bool _isDropDownShowing = false;

		public StudyFilterColumnHeaderCell()
			: base()
		{
			_dataGridViewChangeNotifier = new ChangeNotifier<DataGridView>(
				delegate(DataGridView oldValue, DataGridView newValue)
					{
						if (oldValue != null)
							this.UnsubscribeDataGridViewEvents(oldValue);
					},
				delegate(DataGridView oldValue, DataGridView newValue)
					{
						if (newValue != null)
							this.SubscribeDataGridViewEvents(newValue);
					});
		}

		public StudyFilterColumnHeaderCell(DataGridViewColumnHeaderCell source)
			: this()
		{
			this.ErrorText = source.ErrorText;
			this.Tag = source.Tag;
			this.ToolTipText = source.ToolTipText;
			this.Value = source.Value;
			base.ContextMenuStrip = source.ContextMenuStrip;
			base.ValueType = source.ValueType;

			// Avoid creating a new style object if the Style property has not previously been set. 
			if (source.HasStyle)
				base.Style = source.Style;

			StudyFilterColumnHeaderCell typedSource = source as StudyFilterColumnHeaderCell;
			if (typedSource != null)
			{
				this._dropDownEnabled = typedSource._dropDownEnabled;
			}
		}

		#region Misc Properties

		public bool DropDownEnabled
		{
			get { return _dropDownEnabled; }
			set
			{
				if (_dropDownEnabled != value)
				{
					_dropDownEnabled = value;
					this.ResetDropDown();
				}
			}
		}

		public bool IsDropDownShowing
		{
			get { return _isDropDownShowing; }
			private set
			{
				if (_isDropDownShowing != value)
				{
					_isDropDownShowing = value;

					this.DataGridView.InvalidateCell(this);
				}
			}
		}

		private bool IsProgrammaticSorting
		{
			get { return (this.OwningColumn == null) || (this.OwningColumn.SortMode == DataGridViewColumnSortMode.Programmatic); }
		}

		#endregion

		#region Misc Overrides

		public override object Clone()
		{
			return new StudyFilterColumnHeaderCell(this);
		}

		protected override void OnDataGridViewChanged()
		{
			_dataGridViewChangeNotifier.Value = this.DataGridView;

			// Initialize the drop-down button bounds so that any initial column autosizing will accommodate the button width. 
			SetDropDownButtonBounds();

			base.OnDataGridViewChanged();
		}

		#endregion

		#region Filtered Hot State

		public bool Filtered
		{
			get { return _filtered; }
			set
			{
				if (_filtered != value)
				{
					_filtered = value;
					if (this.DataGridView != null)
						this.DataGridView.InvalidateCell(this);
					EventsHelper.Fire(_filteredChanged, this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler FilteredChanged
		{
			add { _filteredChanged += value; }
			remove { _filteredChanged -= value; }
		}

		#endregion

		#region Painting

		/// <summary>
		/// Paints the column header cell, including the drop-down button. 
		/// </summary>
		/// <param name="graphics">The Graphics used to paint the DataGridViewCell.</param>
		/// <param name="clipBounds">A Rectangle that represents the area of the DataGridView that needs to be repainted.</param>
		/// <param name="cellBounds">A Rectangle that contains the bounds of the DataGridViewCell that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="cellState">A bitwise combination of DataGridViewElementStates values that specifies the state of the cell.</param>
		/// <param name="value">The data of the DataGridViewCell that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the DataGridViewCell that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A DataGridViewCellStyle that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A DataGridViewAdvancedBorderStyle that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of the DataGridViewPaintParts values that specifies which parts of the cell need to be painted.</param>
		protected override void Paint(System.Drawing.Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

			// Continue only if the drop down is to be visible and ContentBackground is part of the paint request. 
			if (!this.IsProgrammaticSorting || (paintParts & DataGridViewPaintParts.ContentBackground) == 0)
			{
				return;
			}

			Rectangle buttonBounds = this.DropDownButtonBounds;
			if (buttonBounds.Width > 0 && buttonBounds.Height > 0) // make sure there's something to draw...
			{
				// Paint the button manually or using visual styles if visual styles 
				// are enabled, using the correct state depending on whether the 
				// filter list is showing and whether there is a filter in effect 
				// for the current column. 
				if (Application.RenderWithVisualStyles)
				{
					ComboBoxState state = ComboBoxState.Normal;
					if (!this.DropDownEnabled)
						state = ComboBoxState.Disabled;
					else if (this.IsDropDownShowing)
						state = ComboBoxState.Pressed;
					else if (_filtered)
						state = ComboBoxState.Hot;
					ComboBoxRenderer.DrawDropDownButton(graphics, buttonBounds, state);
				}
				else
				{
					int pressedOffset = 0;
					PushButtonState state = PushButtonState.Normal;
					if (!this.DropDownEnabled)
					{
						state = PushButtonState.Disabled;
					}
					else if (this.IsDropDownShowing)
					{
						state = PushButtonState.Pressed;
						pressedOffset = 1;
					}
					else if (_filtered)
					{
						state = PushButtonState.Hot;
					}
					ButtonRenderer.DrawButton(graphics, buttonBounds, state);

					// If there is a filter in effect for the column, paint the 
					// down arrow as an unfilled triangle. If there is no filter 
					// in effect, paint the down arrow as a filled triangle.
					if (_filtered)
					{
						graphics.DrawPolygon(this.DropDownEnabled ? SystemPens.ControlText : SystemPens.InactiveCaption,
						                     new Point[]
						                     	{
						                     		new Point(
						                     			buttonBounds.Width/2 +
						                     			buttonBounds.Left - 1 + pressedOffset,
						                     			buttonBounds.Height*3/4 +
						                     			buttonBounds.Top - 1 + pressedOffset),
						                     		new Point(
						                     			buttonBounds.Width/4 +
						                     			buttonBounds.Left + pressedOffset,
						                     			buttonBounds.Height/2 +
						                     			buttonBounds.Top - 1 + pressedOffset),
						                     		new Point(
						                     			buttonBounds.Width*3/4 +
						                     			buttonBounds.Left - 1 + pressedOffset,
						                     			buttonBounds.Height/2 +
						                     			buttonBounds.Top - 1 + pressedOffset)
						                     	});
					}
					else
					{
						graphics.FillPolygon(this.DropDownEnabled ? SystemBrushes.ControlText : SystemBrushes.InactiveCaption,
						                     new Point[]
						                     	{
						                     		new Point(
						                     			buttonBounds.Width/2 +
						                     			buttonBounds.Left - 1 + pressedOffset,
						                     			buttonBounds.Height*3/4 +
						                     			buttonBounds.Top - 1 + pressedOffset),
						                     		new Point(
						                     			buttonBounds.Width/4 +
						                     			buttonBounds.Left + pressedOffset,
						                     			buttonBounds.Height/2 +
						                     			buttonBounds.Top - 1 + pressedOffset),
						                     		new Point(
						                     			buttonBounds.Width*3/4 +
						                     			buttonBounds.Left - 1 + pressedOffset,
						                     			buttonBounds.Height/2 +
						                     			buttonBounds.Top - 1 + pressedOffset)
						                     	});
					}
				}
			}
		}

		#endregion

		#region DataGridView Event Handling

		protected virtual void SubscribeDataGridViewEvents(DataGridView instance)
		{
			instance.Scroll += OnDataGridViewScroll;
			instance.MouseDown += OnDataGridViewMouseDown;
			instance.ColumnDisplayIndexChanged += OnDataGridViewColumnDisplayIndexChanged;
			instance.ColumnWidthChanged += OnDataGridViewColumnWidthChanged;
			instance.ColumnHeadersHeightChanged += OnDataGridViewColumnHeadersHeightChanged;
			instance.RowHeadersWidthChanged += OnDataGridViewRowHeadersWidthChanged;
			instance.SizeChanged += OnDataGridViewSizeChanged;
			instance.DataSourceChanged += OnDataGridViewDataSourceChanged;
			instance.DataBindingComplete += OnDataGridViewDataBindingComplete;
			instance.ColumnSortModeChanged += OnDataGridViewColumnSortModeChanged;
		}

		protected virtual void UnsubscribeDataGridViewEvents(DataGridView instance)
		{
			instance.Scroll -= OnDataGridViewScroll;
			instance.MouseDown -= OnDataGridViewMouseDown;
			instance.ColumnDisplayIndexChanged -= OnDataGridViewColumnDisplayIndexChanged;
			instance.ColumnWidthChanged -= OnDataGridViewColumnWidthChanged;
			instance.ColumnHeadersHeightChanged -= OnDataGridViewColumnHeadersHeightChanged;
			instance.RowHeadersWidthChanged -= OnDataGridViewRowHeadersWidthChanged;
			instance.SizeChanged -= OnDataGridViewSizeChanged;
			instance.DataSourceChanged -= OnDataGridViewDataSourceChanged;
			instance.DataBindingComplete -= OnDataGridViewDataBindingComplete;
			instance.ColumnSortModeChanged -= OnDataGridViewColumnSortModeChanged;
		}

		private void OnDataGridViewColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (e.Column == OwningColumn)
			{
				this.ResetDropDown();

				// this event can change visibility of the button, so we need to reset the cell padding as well here
				this.AdjustPadding(0);
			}
		}

		private void OnDataGridViewScroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				ResetDropDown();
		}

		private void OnDataGridViewDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (e.ListChangedType == ListChangedType.Reset)
				ResetDropDown();
		}

		private void OnDataGridViewMouseDown(object sender, MouseEventArgs e)
		{
			if (!this.DropDownButtonBounds.Contains(e.Location))
				ResetDropDown();
		}

		private void OnDataGridViewColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			ResetDropDown();
		}

		private void OnDataGridViewColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			ResetDropDown();
		}

		private void OnDataGridViewRowHeadersWidthChanged(object sender, EventArgs e)
		{
			ResetDropDown();
		}

		private void OnDataGridViewColumnHeadersHeightChanged(object sender, EventArgs e)
		{
			ResetDropDown();
		}

		private void OnDataGridViewSizeChanged(object sender, EventArgs e)
		{
			ResetDropDown();
		}

		private void OnDataGridViewDataSourceChanged(object sender, EventArgs e)
		{
			ResetDropDown();
		}

		#endregion

		#region Click Handling

		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			// Retrieve the current size and location of the header cell, excluding any portion that is scrolled off screen
			Rectangle cellBounds = this.DataGridView.GetCellDisplayRectangle(e.ColumnIndex, -1, false);

			// Continue only if the column is not manually resizable or the mouse coordinates are not within the column resize zone
			if (this.OwningColumn.Resizable != DataGridViewTriState.True || !((this.DataGridView.RightToLeft == RightToLeft.No && cellBounds.Width - e.X < 6) || e.X < 6))
			{
				// Unless RightToLeft is enabled, store the width of the portion that is scrolled off screen
				int scrollingOffset = 0;
				if (this.DataGridView.RightToLeft == RightToLeft.No && this.DataGridView.FirstDisplayedScrollingColumnIndex == this.ColumnIndex)
				{
					scrollingOffset = this.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				}

				// Show the drop-down list if filtering is enabled and the mouse click occurred
				// within the drop-down button bounds. Otherwise, if sorting is enabled and the
				// click occurred outside the drop-down button bounds, sort by the owning column. 
				// The mouse coordinates are relative to the cell bounds, so the cell location
				// and the scrolling offset are needed to determine the client coordinates.
				if (this.IsProgrammaticSorting && this.DropDownButtonBounds.Contains(e.X + cellBounds.Left - scrollingOffset, e.Y + cellBounds.Top))
				{
					if (this.DropDownEnabled)
					{
						if (!this.IsDropDownShowing)
							ShowDropDown();
						else
							HideDropDown();
					}
				}
			}

			base.OnMouseDown(e);
		}

		#endregion

		#region DropDown Handling

		public void ShowDropDown()
		{
			if (!this.IsDropDownShowing)
			{
				if (this.OnDropDownShowing())
				{
					this.IsDropDownShowing = true;
					this.OnDropDownShown();
				}
			}
		}

		public void HideDropDown()
		{
			if (this.IsDropDownShowing)
			{
				if (this.OnDropDownHiding())
				{
					this.IsDropDownShowing = false;
					this.OnDropDownHidden();
				}
			}
		}

		public void ResetDropDown()
		{
			this.InvalidateDropDownButtonBounds();
			this.HideDropDown();
			if(this.DataGridView != null)
				this.DataGridView.InvalidateCell(this);
		}

		public event CancelEventHandler DropDownShowing;
		public event EventHandler DropDownShown;
		public event CancelEventHandler DropDownHiding;
		public event EventHandler DropDownHidden;

		protected virtual bool OnDropDownShowing()
		{
			CancelEventArgs e = new CancelEventArgs();
			if (this.DropDownShowing != null)
				this.DropDownShowing(this, e);

			if (!e.Cancel && this.DataGridView != null)
			{
				// If the current cell is in edit mode, commit the edit. 
				if (this.DataGridView.IsCurrentCellInEditMode)
				{
					// Commit and end the cell edit.  
					this.DataGridView.EndEdit();
				}

				// Ensure that the current row is not the row for new records.
				// This prevents the new row from affecting the filter list and also
				// prevents the new row from being added when the filter changes.
				if (this.DataGridView.CurrentRow != null && this.DataGridView.CurrentRow.IsNewRow)
				{
					this.DataGridView.CurrentCell = null;
				}
			}

			return !e.Cancel;
		}

		protected virtual void OnDropDownShown()
		{
			if (this.DropDownShown != null)
				this.DropDownShown(this, EventArgs.Empty);
		}

		protected virtual bool OnDropDownHiding()
		{
			CancelEventArgs e = new CancelEventArgs();
			if (this.DropDownHiding != null)
				this.DropDownHiding(this, e);
			return !e.Cancel;
		}

		protected virtual void OnDropDownHidden()
		{
			if (this.DropDownHidden != null)
				this.DropDownHidden(this, EventArgs.Empty);
		}

		#endregion

		#region Private Members for computing drop down button positioning

		protected Rectangle DropDownButtonBounds
		{
			get
			{
				if (!this.IsProgrammaticSorting)
					return Rectangle.Empty;

				if (_dropDownButtonBounds == Rectangle.Empty)
					this.SetDropDownButtonBounds();
				return _dropDownButtonBounds;
			}
		}

		private void InvalidateDropDownButtonBounds()
		{
			_dropDownButtonBounds = Rectangle.Empty;
		}

		/// <summary>
		/// Sets the position and size of _dropDownButtonBounds based on the current 
		/// cell bounds and the preferred cell height for a single line of header text. 
		/// </summary>
		private void SetDropDownButtonBounds()
		{
			if (this.DataGridView == null)
			{
				_dropDownButtonBounds = Rectangle.Empty;
				_dropDownButtonPaddingOffset = 0;
				return;
			}

			// Retrieve the cell display rectangle, which is used to set the position of the drop-down button
			Rectangle cellBounds = this.DataGridView.GetCellDisplayRectangle(this.ColumnIndex, -1, false);

			// Initialize a variable to store the button edge length, setting its initial value based on the font height
			int buttonEdgeLength = this.InheritedStyle.Font.Height + 5;

			// Calculate the height of the cell borders and padding
			Rectangle borderRect = this.BorderWidths(this.DataGridView.AdjustColumnHeaderBorderStyle(this.DataGridView.AdvancedColumnHeadersBorderStyle, new DataGridViewAdvancedBorderStyle(), false, false));
			int borderAndPaddingHeight = 2 + borderRect.Top + borderRect.Height + this.InheritedStyle.Padding.Vertical;
			bool visualStylesEnabled = Application.RenderWithVisualStyles && this.DataGridView.EnableHeadersVisualStyles;
			if (visualStylesEnabled)
			{
				borderAndPaddingHeight += 3;
			}

			// Constrain the button edge length to the height of the column headers minus the border and padding height
			if (buttonEdgeLength > this.DataGridView.ColumnHeadersHeight - borderAndPaddingHeight)
			{
				buttonEdgeLength = this.DataGridView.ColumnHeadersHeight - borderAndPaddingHeight;
			}

			// Constrain the button edge length to the width of the cell minus three
			if (buttonEdgeLength > cellBounds.Width - 3)
			{
				buttonEdgeLength = cellBounds.Width - 3;
			}

			// Calculate the location of the drop-down button, with adjustments based on whether visual styles are enabled
			int topOffset = visualStylesEnabled ? 4 : 1;
			int top = cellBounds.Bottom - buttonEdgeLength - topOffset;
			int leftOffset = visualStylesEnabled ? 3 : 1;
			int left = 0;
			if (this.DataGridView.RightToLeft == RightToLeft.No)
			{
				left = cellBounds.Right - buttonEdgeLength - leftOffset;
			}
			else
			{
				left = cellBounds.Left + leftOffset;
			}

			// Set the bounds using the calculated values, and adjust the cell padding accordingly
			_dropDownButtonBounds = new Rectangle(left, top, buttonEdgeLength, buttonEdgeLength);
			this.AdjustPadding(buttonEdgeLength + leftOffset);
		}

		/// <summary>
		/// Adjusts the cell padding to widen the header by the drop-down button width.
		/// </summary>
		/// <param name="newDropDownButtonPaddingOffset">The new drop-down button width.</param>
		private void AdjustPadding(int newDropDownButtonPaddingOffset)
		{
			// Determine the difference between the new and current padding adjustment.
			int widthChange = newDropDownButtonPaddingOffset - _dropDownButtonPaddingOffset;

			// If the padding needs to change, store the new value and make the change.
			if (widthChange != 0)
			{
				// Store the offset for the drop-down button separately from the padding in case the client needs additional padding.
				_dropDownButtonPaddingOffset = newDropDownButtonPaddingOffset;

				// Create a new Padding using the adjustment amount, then add it to the cell's existing Style.Padding property value. 
				Padding dropDownPadding = new Padding(0, 0, widthChange, 0);
				this.Style.Padding = Padding.Add(this.InheritedStyle.Padding, dropDownPadding);
			}
		}

		#endregion
	}
}