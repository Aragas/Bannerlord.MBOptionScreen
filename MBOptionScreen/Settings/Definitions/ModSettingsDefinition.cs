namespace MBOptionScreen.Settings
{
    public class ModSettingsDefinition
    {
        public string SettingsId { get; }
        public SettingsBase SettingsInstance => SettingsDatabase.GetSettings(SettingsId);
        public string ModName => SettingsInstance.ModName;

        public ModSettingsDefinition(string settingsId)
        {
            SettingsId = settingsId;
        }
    }
}