﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeLapse_GUI {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class TimeLapseGUISettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static TimeLapseGUISettings defaultInstance = ((TimeLapseGUISettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new TimeLapseGUISettings())));
        
        public static TimeLapseGUISettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11/27/2013 05:00:00")]
        public global::System.DateTime StartTime {
            get {
                return ((global::System.DateTime)(this["StartTime"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11/27/2013 21:00:00")]
        public global::System.DateTime StopTime {
            get {
                return ((global::System.DateTime)(this["StopTime"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public int GrabFrequency {
            get {
                return ((int)(this["GrabFrequency"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://192.168.0.20/snapshot.cgi?chan=0")]
        public string CameraURL {
            get {
                return ((string)(this["CameraURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://test.everinview.com/")]
        public string UploadURL {
            get {
                return ((string)(this["UploadURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int ServerWorkerThreadCount {
            get {
                return ((int)(this["ServerWorkerThreadCount"]));
            }
        }
    }
}