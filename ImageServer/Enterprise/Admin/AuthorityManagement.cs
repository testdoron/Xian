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
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Common.Admin.AuthorityGroupAdmin;

namespace ClearCanvas.ImageServer.Enterprise.Admin
{
    /// <summary>
    /// Wrapper for <see cref="IAuthorityGroupAdminService"/> service.
    /// </summary>
    public sealed class AuthorityManagement : IDisposable
    {
        private IAuthorityGroupAdminService _service;

        public AuthorityManagement()
        {
            _service =  Platform.GetService<IAuthorityGroupAdminService>();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_service != null && _service is IDisposable)
            {
                (_service as IDisposable).Dispose();
                _service = null;
            }
        }

        #endregion



        public IList<AuthorityGroupSummary> ListAllAuthorityGroups()
        {
            return _service.ListAuthorityGroups(new ListAuthorityGroupsRequest()).AuthorityGroups;
        }

        public void AddAuthorityGroup(string name, List<AuthorityTokenSummary> tokens)
        {
            AuthorityGroupDetail details = new AuthorityGroupDetail();
            details.Name = name;
            details.AuthorityTokens = tokens;
            _service.AddAuthorityGroup(new AddAuthorityGroupRequest(details));
        }

        public void UpdateAuthorityGroup(AuthorityGroupDetail detail)
        {
            _service.UpdateAuthorityGroup(new UpdateAuthorityGroupRequest(detail));
        }

        public void DeleteAuthorityGroup(EntityRef entityRef)
        {
            _service.DeleteAuthorityGroup(new DeleteAuthorityGroupRequest(entityRef));
        }

        public void ImportAuthorityTokens(List<AuthorityTokenSummary> tokens)
        {
            _service.ImportAuthorityTokens(new ImportAuthorityTokensRequest(tokens));
        }

        public AuthorityGroupDetail LoadAuthorityGroupDetail(AuthorityGroupSummary group)
        {
            return
                _service.LoadAuthorityGroupForEdit(new LoadAuthorityGroupForEditRequest(group.AuthorityGroupRef)).
                    AuthorityGroupDetail;
        }

        public IList<AuthorityTokenSummary> ListAuthorityTokens()
        {
            return _service.ListAuthorityTokens(new ListAuthorityTokensRequest()).AuthorityTokens;
        }

        public bool ImportAuthorityGroups(List<AuthorityGroupDetail> groups)
        {
            ImportAuthorityGroupsRequest request = new ImportAuthorityGroupsRequest(groups);
            return _service.ImportAuthorityGroups(request)!=null;
        }
    }
}