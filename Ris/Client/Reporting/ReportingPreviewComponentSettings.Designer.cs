﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Client.Reporting {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class ReportingPreviewComponentSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ReportingPreviewComponentSettings defaultInstance = ((ReportingPreviewComponentSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ReportingPreviewComponentSettings())));
        
        public static ReportingPreviewComponentSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost/RIS/radiologistpreview.htm")]
        public string DetailsPageUrl {
            get {
                return ((string)(this["DetailsPageUrl"]));
            }
        }
    }
}
