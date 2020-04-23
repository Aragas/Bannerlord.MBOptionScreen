using MBOptionScreen.Settings;

using System.Collections.Generic;

// Keep namespace
namespace MBOptionScreen.Interfaces
{
    public interface ISettingsProvider
    {
        List<ModSettingsDefinition> CreateModSettingsDefinitions { get; }

        bool RegisterSettings(SettingsBase settingsClass);
        SettingsBase? GetSettings(string uniqueId);
        void SaveSettings(SettingsBase settingsInstance);
        bool OverrideSettings(SettingsBase settings);
        SettingsBase? ResetSettings(string id);
    }
}