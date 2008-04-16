using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Enterprise.Common;
using System.Runtime.Serialization;

namespace ClearCanvas.Ris.Application.Common.OrderNotes
{
    [DataContract]
    public class AcknowledgeAndReplyResponse : DataContractBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="orderNote"></param>
        public AcknowledgeAndReplyResponse(OrderNoteDetail orderNote)
        {
            OrderNote = orderNote;
        }

        /// <summary>
        /// Returns the order note that was created as a result of the request.
        /// </summary>
        [DataMember]
        public OrderNoteDetail OrderNote;
    }
}
