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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ClearCanvas.Desktop.View.WinForms;
using ClearCanvas.ImageViewer.Utilities.StudyFilters.Export;

namespace ClearCanvas.ImageViewer.Utilities.StudyFilters.View.WinForms
{
	public partial class ExportComponentPanel : ApplicationComponentUserControl
	{
		private readonly ExportComponent _component;

		public ExportComponentPanel(ExportComponent component)
			: base(component)
		{
			InitializeComponent();

			_component = component;

			base.CancelButton = _cancelButton;
			base.AcceptButton = _okButton;

			_patientId.DataBindings.Add("Value", _component, "PatientId", true, DataSourceUpdateMode.OnPropertyChanged);
			_patientsName.DataBindings.Add("Value", _component, "PatientsName", true, DataSourceUpdateMode.OnPropertyChanged);
			_dateOfBirth.DataBindings.Add("Value", _component, "PatientsDateOfBirth", true, DataSourceUpdateMode.OnPropertyChanged);
			_studyId.DataBindings.Add("Value", _component, "StudyId", true, DataSourceUpdateMode.OnPropertyChanged);
			_studyDescription.DataBindings.Add("Value", _component, "StudyDescription", true, DataSourceUpdateMode.OnPropertyChanged);
			_accessionNumber.DataBindings.Add("Value", _component, "AccessionNumber", true, DataSourceUpdateMode.OnPropertyChanged);
			_studyDate.DataBindings.Add("Value", _component, "StudyDate", true, DataSourceUpdateMode.OnPropertyChanged);
			_outputPath.DataBindings.Add("Text", _component, "OutputPath", true, DataSourceUpdateMode.OnPropertyChanged);

			base.ErrorProvider.SetIconAlignment(_outputPath, ErrorIconAlignment.MiddleLeft);
		}

		private void _browse_Click(object sender, EventArgs e)
		{
			_component.ShowOutputPathDialog();
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			_component.Accept();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			_component.Cancel();
		}
	}
}
