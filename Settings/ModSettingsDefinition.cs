namespace MBOptionScreen.Settings
{
    public class ModSettingsDefinition
    {
        public SettingsBase SettingsInstance { get; set; }
        public string ModName => SettingsInstance.ModName;

        public ModSettingsDefinition(SettingsBase settings)
        {
            SettingsInstance = settings;
        }
    }
}