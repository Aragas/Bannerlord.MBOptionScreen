using MCM.Abstractions.Settings.SettingsProvider;

namespace MCM.Abstractions.Settings.Definitions
{
    public class SettingsDefinition
    {
        public string SettingsId { get; }
        public string ModName { get; }

        public SettingsDefinition(string settingsId)
        {
            var settings = BaseSettingsProvider.Instance.GetSettings(settingsId);

            SettingsId = settingsId;
            ModName = settings?.ModName ?? "ERROR";
        }
    }
}