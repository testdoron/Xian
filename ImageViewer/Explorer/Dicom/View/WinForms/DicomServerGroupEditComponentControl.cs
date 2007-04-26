using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.ImageViewer.Explorer.Dicom.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms user-interface for <see cref="DicomServerGroupEditComponent"/>
    /// </summary>
    public partial class DicomServerGroupEditComponentControl : CustomUserControl
    {
        private DicomServerGroupEditComponent _component;
        private BindingSource _bindingSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public DicomServerGroupEditComponentControl(DicomServerGroupEditComponent component)
        {
            InitializeComponent();

            _component = component;

			this.AcceptButton = _btnAccept;
			this.CancelButton = _btnCancel;
			
			AcceptClicked += new EventHandler(OnAcceptClicked);
            CancelClicked += new EventHandler(OnCancelClicked);

            _bindingSource = new BindingSource();
            _bindingSource.DataSource = _component;
            this._serverGroupName.DataBindings.Add("Text", _bindingSource, "ServerGroupName", true, DataSourceUpdateMode.OnPropertyChanged);
            _btnAccept.DataBindings.Add("Enabled", _component, "AcceptEnabled", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public event EventHandler AcceptClicked
        {
            add { _btnAccept.Click += value; }
            remove { _btnAccept.Click -= value; }
        }

        public event EventHandler CancelClicked
        {
            add { _btnCancel.Click += value; }
            remove { _btnCancel.Click -= value; }
        }

        private void OnAcceptClicked(object sender, EventArgs e)
        {
            try
            {
                _component.Accept();
            }
            catch (DicomServerException dse)
            {
                Platform.ShowMessageBox(dse.Message.ToString(), MessageBoxActions.Ok);
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            _component.Cancel();
        }
    }
}
