﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AudioLoopBack {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.3.0.0")]
    internal sealed partial class GlobalSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static GlobalSettings defaultInstance = ((GlobalSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new GlobalSettings())));
        
        public static GlobalSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkOrange")]
        public global::System.Drawing.Color Background_Color_Spect_C {
            get {
                return ((global::System.Drawing.Color)(this["Background_Color_Spect_C"]));
            }
            set {
                this["Background_Color_Spect_C"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Turquoise")]
        public global::System.Drawing.Color Background_Color_Wave_C {
            get {
                return ((global::System.Drawing.Color)(this["Background_Color_Wave_C"]));
            }
            set {
                this["Background_Color_Wave_C"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowCursor {
            get {
                return ((bool)(this["ShowCursor"]));
            }
            set {
                this["ShowCursor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.1")]
        public float Cursor_L {
            get {
                return ((float)(this["Cursor_L"]));
            }
            set {
                this["Cursor_L"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.9")]
        public float Cursor_R {
            get {
                return ((float)(this["Cursor_R"]));
            }
            set {
                this["Cursor_R"] = value;
            }
        }
    }
}
