namespace AudioLoopBack {
    
    
  
    //  SettingChanging 
    //  PropertyChanged 
    //  SettingsLoaded 
    //  SettingsSaving 
    internal sealed partial class GlobalSettings {
        
        public GlobalSettings() {
            // // 
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            //  SettingChangingEvent 
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // SettingsSaving 
        }
    }
}
