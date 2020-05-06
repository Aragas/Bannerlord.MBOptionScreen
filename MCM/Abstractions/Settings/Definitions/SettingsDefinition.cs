using MCM.Abstractions.Settings.SettingsProvider;

namespace MCM.Abstractions.Settings.Definitions
{
    public class SettingsDefinition
    {
        public string SettingsId { get; }
        public string DisplayName { get; }

        public SettingsDefinition(string settingsId)
        {
            var settings = BaseSettingsProvider.Instance.GetSettings(settingsId);

            SettingsId = settingsId;
            DisplayName = settings?.DisplayName ?? "ERROR";
        }
    }
}