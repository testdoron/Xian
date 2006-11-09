using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Healthcare;
using ClearCanvas.Enterprise;
using ClearCanvas.Ris.Services;
using ClearCanvas.Desktop.Tables;

namespace ClearCanvas.Ris.Client.Adt
{
    /// <summary>
    /// Extension point for views onto <see cref="ConfirmReconciliationComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class ConfirmReconciliationComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// ConfirmReconciliationComponent class
    /// </summary>
    [AssociateView(typeof(ConfirmReconciliationComponentViewExtensionPoint))]
    public class ReconciliationConfirmComponent : ApplicationComponent
    {
        private PatientProfileTable _sourceProfiles;
        private PatientProfileTable _targetProfiles;


        /// <summary>
        /// Constructor
        /// </summary>
        public ReconciliationConfirmComponent(Patient target, IList<Patient> sources)
        {
            _sourceProfiles = new PatientProfileTable();
            foreach (Patient patient in sources)
            {
                _sourceProfiles.Items.AddRange(patient.Profiles);
            }

            _targetProfiles = new PatientProfileTable();
            _targetProfiles.Items.AddRange(target.Profiles);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public ITable SourcePatientData
        {
            get { return _sourceProfiles; }
        }

        public ITable TargetPatientData
        {
            get { return _targetProfiles; }
        }

        public void Continue()
        {
            this.ExitCode = ApplicationComponentExitCode.Normal;
            this.Host.Exit();
        }

        public void Cancel()
        {
            this.ExitCode = ApplicationComponentExitCode.Cancelled;
            this.Host.Exit();
        }

    }
}
