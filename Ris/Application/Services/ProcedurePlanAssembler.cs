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

using System.Collections.Generic;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Workflow;
using Iesi.Collections.Generic;

namespace ClearCanvas.Ris.Application.Services
{
    public class ProcedurePlanAssembler
    {
        public ProcedurePlanDetail CreateProcedurePlanSummary(Order order, IPersistenceContext context)
        {
            ProcedurePlanDetail detail = new ProcedurePlanDetail();

            ProcedureAssembler assembler = new ProcedureAssembler();
            StaffAssembler staffAssembler = new StaffAssembler();

            detail.OrderRef = order.GetRef();
            detail.Procedures = CollectionUtils.Map<Procedure, ProcedureDetail>(
                order.Procedures,
                delegate(Procedure rp)
                {
                	return assembler.CreateProcedureDetail(
                		rp,
                		delegate(ProcedureStep ps) { return ps.Is<ModalityProcedureStep>(); }, // only MPS are relevant here
                		false,
                		context);
                });
            detail.DiagnosticServiceSummary =
                new DiagnosticServiceSummary(order.DiagnosticService.GetRef(), order.DiagnosticService.Id, order.DiagnosticService.Name, order.DiagnosticService.Deactivated);


            return detail;
        }
    }
}
