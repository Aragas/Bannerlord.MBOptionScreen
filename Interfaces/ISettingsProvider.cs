using MBOptionScreen.Settings;

using System.Collections.Generic;

namespace MBOptionScreen.Interfaces
{
    public interface ISettingsProvider
    {
        List<ModSettingsDefinition> CreateModSettingsDefinitions { get; }

        bool RegisterSettings(SettingsBase settingsClass);
        SettingsBase? GetSettings(string uniqueId);
        void SaveSettings(SettingsBase settingsInstance);
        bool OverrideSettingsWithId(SettingsBase settings, string Id);
        SettingsBase ResetSettingsInstance(SettingsBase settingsInstance);
    }
}