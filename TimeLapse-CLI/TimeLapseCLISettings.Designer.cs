﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeLapse_CLI {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class TimeLapseCLISettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static TimeLapseCLISettings defaultInstance = ((TimeLapseCLISettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new TimeLapseCLISettings())));
        
        public static TimeLapseCLISettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("05/29/2014 05:00:00")]
        public global::System.DateTime StartTime {
            get {
                return ((global::System.DateTime)(this["StartTime"]));
            }
            set {
                this["StartTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("05/29/2014 21:00:00")]
        public global::System.DateTime StopTime {
            get {
                return ((global::System.DateTime)(this["StopTime"]));
            }
            set {
                this["StopTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public int GrabFrequency {
            get {
                return ((int)(this["GrabFrequency"]));
            }
            set {
                this["GrabFrequency"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://test.everinview.com")]
        public string WebURL {
            get {
                return ((string)(this["WebURL"]));
            }
            set {
                this["WebURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1280")]
        public int ImageWidth {
            get {
                return ((int)(this["ImageWidth"]));
            }
            set {
                this["ImageWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("720")]
        public int ImageHeight {
            get {
                return ((int)(this["ImageHeight"]));
            }
            set {
                this["ImageHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("90")]
        public int ImageRotation {
            get {
                return ((int)(this["ImageRotation"]));
            }
            set {
                this["ImageRotation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public int ImageQuality {
            get {
                return ((int)(this["ImageQuality"]));
            }
            set {
                this["ImageQuality"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int CameraID {
            get {
                return ((int)(this["CameraID"]));
            }
            set {
                this["CameraID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int ServerWorkerThreadCount {
            get {
                return ((int)(this["ServerWorkerThreadCount"]));
            }
            set {
                this["ServerWorkerThreadCount"] = value;
            }
        }
    }
}