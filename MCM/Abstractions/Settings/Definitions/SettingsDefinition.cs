using MCM.Abstractions.Settings.SettingsProvider;

namespace MCM.Abstractions.Settings.Definitions
{
    // TODO
    public class SettingsDefinition
    {
        public string SettingsId { get; }
        private SettingsBase SettingsInstance => BaseSettingsProvider.Instance.GetSettings(SettingsId);
        public string ModName => SettingsInstance.ModName;

        public SettingsDefinition(string settingsId)
        {
            SettingsId = settingsId;
        }
    }
}