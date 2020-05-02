using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public interface ISettingsContainer
    {
        List<SettingsDefinition> CreateModSettingsDefinitions { get; }

        bool RegisterSettings(SettingsBase settingsClass);
        SettingsBase? GetSettings(string uniqueId);
        void SaveSettings(SettingsBase settingsInstance);
        bool OverrideSettings(SettingsBase settings);
        SettingsBase? ResetSettings(string id);
    }
}