using System;
using System.ServiceModel;

namespace ClearCanvas.Ris.Application.Common.RegistrationWorkflow
{
    [ServiceContract]
    public interface IRegistrationWorkflowService
    {
        [OperationContract]
        GetWorklistResponse GetWorklist(GetWorklistRequest request);

        [OperationContract]
        LoadWorklistPreviewResponse LoadWorklistPreview(LoadWorklistPreviewRequest request);

        [OperationContract]
        GetOperationEnablementResponse GetOperationEnablement(GetOperationEnablementRequest request);

        [OperationContract(IsOneWay = true)]
        void ExecuteOperation(ExecuteOperationRequest request);

        [OperationContract]
        GetDataForCheckInTableResponse GetDataForCheckInTable(GetDataForCheckInTableRequest request);

        [OperationContract(IsOneWay = true)]
        void CheckInProcedure(CheckInProcedureRequest request);
    }
}
