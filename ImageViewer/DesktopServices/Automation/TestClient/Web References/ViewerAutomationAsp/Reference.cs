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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.3082.
// 
#pragma warning disable 1591

namespace ClearCanvas.ImageViewer.DesktopServices.Automation.TestClient.ViewerAutomationAsp {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ViewerAutomation", Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class ViewerAutomation : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetActiveViewersOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetViewerInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback OpenStudiesOperationCompleted;
        
        private System.Threading.SendOrPostCallback ActivateViewerOperationCompleted;
        
        private System.Threading.SendOrPostCallback CloseViewerOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ViewerAutomation() {
            this.Url = "http://localhost:51124/ClearCanvas/ImageViewer/Automation";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetActiveViewersCompletedEventHandler GetActiveViewersCompleted;
        
        /// <remarks/>
        public event GetViewerInfoCompletedEventHandler GetViewerInfoCompleted;
        
        /// <remarks/>
        public event OpenStudiesCompletedEventHandler OpenStudiesCompleted;
        
        /// <remarks/>
        public event ActivateViewerCompletedEventHandler ActivateViewerCompleted;
        
        /// <remarks/>
        public event CloseViewerCompletedEventHandler CloseViewerCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.clearcanvas.ca/imageViewer/automation/IViewerAutomation/GetActiveViewe" +
            "rs", RequestNamespace="http://www.clearcanvas.ca/imageViewer/automation", ResponseNamespace="http://www.clearcanvas.ca/imageViewer/automation", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public GetActiveViewersResult GetActiveViewers() {
            object[] results = this.Invoke("GetActiveViewers", new object[0]);
            return ((GetActiveViewersResult)(results[0]));
        }
        
        /// <remarks/>
        public void GetActiveViewersAsync() {
            this.GetActiveViewersAsync(null);
        }
        
        /// <remarks/>
        public void GetActiveViewersAsync(object userState) {
            if ((this.GetActiveViewersOperationCompleted == null)) {
                this.GetActiveViewersOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetActiveViewersOperationCompleted);
            }
            this.InvokeAsync("GetActiveViewers", new object[0], this.GetActiveViewersOperationCompleted, userState);
        }
        
        private void OnGetActiveViewersOperationCompleted(object arg) {
            if ((this.GetActiveViewersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetActiveViewersCompleted(this, new GetActiveViewersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.clearcanvas.ca/imageViewer/automation/IViewerAutomation/GetViewerInfo", RequestNamespace="http://www.clearcanvas.ca/imageViewer/automation", ResponseNamespace="http://www.clearcanvas.ca/imageViewer/automation", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public GetViewerInfoResult GetViewerInfo([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] GetViewerInfoRequest request) {
            object[] results = this.Invoke("GetViewerInfo", new object[] {
                        request});
            return ((GetViewerInfoResult)(results[0]));
        }
        
        /// <remarks/>
        public void GetViewerInfoAsync(GetViewerInfoRequest request) {
            this.GetViewerInfoAsync(request, null);
        }
        
        /// <remarks/>
        public void GetViewerInfoAsync(GetViewerInfoRequest request, object userState) {
            if ((this.GetViewerInfoOperationCompleted == null)) {
                this.GetViewerInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetViewerInfoOperationCompleted);
            }
            this.InvokeAsync("GetViewerInfo", new object[] {
                        request}, this.GetViewerInfoOperationCompleted, userState);
        }
        
        private void OnGetViewerInfoOperationCompleted(object arg) {
            if ((this.GetViewerInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetViewerInfoCompleted(this, new GetViewerInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.clearcanvas.ca/imageViewer/automation/IViewerAutomation/OpenStudies", RequestNamespace="http://www.clearcanvas.ca/imageViewer/automation", ResponseNamespace="http://www.clearcanvas.ca/imageViewer/automation", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public OpenStudiesResult OpenStudies([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] OpenStudiesRequest request) {
            object[] results = this.Invoke("OpenStudies", new object[] {
                        request});
            return ((OpenStudiesResult)(results[0]));
        }
        
        /// <remarks/>
        public void OpenStudiesAsync(OpenStudiesRequest request) {
            this.OpenStudiesAsync(request, null);
        }
        
        /// <remarks/>
        public void OpenStudiesAsync(OpenStudiesRequest request, object userState) {
            if ((this.OpenStudiesOperationCompleted == null)) {
                this.OpenStudiesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnOpenStudiesOperationCompleted);
            }
            this.InvokeAsync("OpenStudies", new object[] {
                        request}, this.OpenStudiesOperationCompleted, userState);
        }
        
        private void OnOpenStudiesOperationCompleted(object arg) {
            if ((this.OpenStudiesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.OpenStudiesCompleted(this, new OpenStudiesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.clearcanvas.ca/imageViewer/automation/IViewerAutomation/ActivateViewer" +
            "", RequestNamespace="http://www.clearcanvas.ca/imageViewer/automation", ResponseNamespace="http://www.clearcanvas.ca/imageViewer/automation", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ActivateViewer([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] ActivateViewerRequest request) {
            this.Invoke("ActivateViewer", new object[] {
                        request});
        }
        
        /// <remarks/>
        public void ActivateViewerAsync(ActivateViewerRequest request) {
            this.ActivateViewerAsync(request, null);
        }
        
        /// <remarks/>
        public void ActivateViewerAsync(ActivateViewerRequest request, object userState) {
            if ((this.ActivateViewerOperationCompleted == null)) {
                this.ActivateViewerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnActivateViewerOperationCompleted);
            }
            this.InvokeAsync("ActivateViewer", new object[] {
                        request}, this.ActivateViewerOperationCompleted, userState);
        }
        
        private void OnActivateViewerOperationCompleted(object arg) {
            if ((this.ActivateViewerCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ActivateViewerCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.clearcanvas.ca/imageViewer/automation/IViewerAutomation/CloseViewer", RequestNamespace="http://www.clearcanvas.ca/imageViewer/automation", ResponseNamespace="http://www.clearcanvas.ca/imageViewer/automation", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CloseViewer([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] CloseViewerRequest request) {
            this.Invoke("CloseViewer", new object[] {
                        request});
        }
        
        /// <remarks/>
        public void CloseViewerAsync(CloseViewerRequest request) {
            this.CloseViewerAsync(request, null);
        }
        
        /// <remarks/>
        public void CloseViewerAsync(CloseViewerRequest request, object userState) {
            if ((this.CloseViewerOperationCompleted == null)) {
                this.CloseViewerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCloseViewerOperationCompleted);
            }
            this.InvokeAsync("CloseViewer", new object[] {
                        request}, this.CloseViewerOperationCompleted, userState);
        }
        
        private void OnCloseViewerOperationCompleted(object arg) {
            if ((this.CloseViewerCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CloseViewerCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class GetActiveViewersResult {
        
        private Viewer[] activeViewersField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        public Viewer[] ActiveViewers {
            get {
                return this.activeViewersField;
            }
            set {
                this.activeViewersField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class Viewer {
        
        private string identifierField;
        
        private string primaryStudyInstanceUidField;
        
        /// <remarks/>
        public string Identifier {
            get {
                return this.identifierField;
            }
            set {
                this.identifierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PrimaryStudyInstanceUid {
            get {
                return this.primaryStudyInstanceUidField;
            }
            set {
                this.primaryStudyInstanceUidField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class CloseViewerRequest {
        
        private Viewer viewerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Viewer Viewer {
            get {
                return this.viewerField;
            }
            set {
                this.viewerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class ActivateViewerRequest {
        
        private Viewer viewerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Viewer Viewer {
            get {
                return this.viewerField;
            }
            set {
                this.viewerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class OpenStudiesResult {
        
        private Viewer viewerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Viewer Viewer {
            get {
                return this.viewerField;
            }
            set {
                this.viewerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class OpenStudyInfo {
        
        private string sourceAETitleField;
        
        private string studyInstanceUidField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string SourceAETitle {
            get {
                return this.sourceAETitleField;
            }
            set {
                this.sourceAETitleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string StudyInstanceUid {
            get {
                return this.studyInstanceUidField;
            }
            set {
                this.studyInstanceUidField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class OpenStudiesRequest {
        
        private System.Nullable<bool> activateIfAlreadyOpenField;
        
        private bool activateIfAlreadyOpenFieldSpecified;
        
        private OpenStudyInfo[] studiesToOpenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> ActivateIfAlreadyOpen {
            get {
                return this.activateIfAlreadyOpenField;
            }
            set {
                this.activateIfAlreadyOpenField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActivateIfAlreadyOpenSpecified {
            get {
                return this.activateIfAlreadyOpenFieldSpecified;
            }
            set {
                this.activateIfAlreadyOpenFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        public OpenStudyInfo[] StudiesToOpen {
            get {
                return this.studiesToOpenField;
            }
            set {
                this.studiesToOpenField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class GetViewerInfoResult {
        
        private string[] additionalStudyInstanceUidsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] AdditionalStudyInstanceUids {
            get {
                return this.additionalStudyInstanceUidsField;
            }
            set {
                this.additionalStudyInstanceUidsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.clearcanvas.ca/imageViewer/automation")]
    public partial class GetViewerInfoRequest {
        
        private Viewer viewerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Viewer Viewer {
            get {
                return this.viewerField;
            }
            set {
                this.viewerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void GetActiveViewersCompletedEventHandler(object sender, GetActiveViewersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetActiveViewersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetActiveViewersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GetActiveViewersResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GetActiveViewersResult)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void GetViewerInfoCompletedEventHandler(object sender, GetViewerInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetViewerInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetViewerInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GetViewerInfoResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GetViewerInfoResult)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void OpenStudiesCompletedEventHandler(object sender, OpenStudiesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class OpenStudiesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal OpenStudiesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public OpenStudiesResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((OpenStudiesResult)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void ActivateViewerCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void CloseViewerCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591