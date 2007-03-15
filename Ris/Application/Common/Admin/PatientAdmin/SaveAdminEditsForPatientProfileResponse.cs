using System;
using System.Runtime.Serialization;

using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;

namespace ClearCanvas.Ris.Application.Common.Admin.PatientAdmin
{
    [DataContract]
    public class SaveAdminEditsForPatientProfileResponse : DataContractBase
    {
        public SaveAdminEditsForPatientProfileResponse(EntityRef patientProfileRef, RegistrationWorklistItem worklistItem)
        {
            this.PatientProfileRef = patientProfileRef;
            this.WorklistItem = worklistItem;
        }

        [DataMember]
        public EntityRef PatientProfileRef;

        [DataMember]
        public RegistrationWorklistItem WorklistItem;
    }
}
