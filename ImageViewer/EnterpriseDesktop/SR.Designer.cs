﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.EnterpriseDesktop {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SR {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SR() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.ImageViewer.EnterpriseDesktop.SR", typeof(SR).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Change Password.
        /// </summary>
        internal static string MenuChangePassword {
            get {
                return ResourceManager.GetString("MenuChangePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Access denied.  Please ensure your username and password are correct, or contact a system administrator for assistance..
        /// </summary>
        internal static string MessageAccessDenied {
            get {
                return ResourceManager.GetString("MessageAccessDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A communication error occured while attempting to contact the server..
        /// </summary>
        internal static string MessageCommunicationError {
            get {
                return ResourceManager.GetString("MessageCommunicationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timed-out while attempting to login..
        /// </summary>
        internal static string MessageLoginTimeout {
            get {
                return ResourceManager.GetString("MessageLoginTimeout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to connect to the server..
        /// </summary>
        internal static string MessageNoEndpointListening {
            get {
                return ResourceManager.GetString("MessageNoEndpointListening", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No service provider was found.  There may be an issue with the application&apos;s configuration..
        /// </summary>
        internal static string MessageNoServiceProvider {
            get {
                return ResourceManager.GetString("MessageNoServiceProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password changed..
        /// </summary>
        internal static string MessagePasswordChanged {
            get {
                return ResourceManager.GetString("MessagePasswordChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown error occured while attempting to communicate with the server.  See log for details..
        /// </summary>
        internal static string MessageUnknownErrorCommunicatingWithServer {
            get {
                return ResourceManager.GetString("MessageUnknownErrorCommunicatingWithServer", resourceCulture);
            }
        }
    }
}