using MBOptionScreen.Settings;

using System.Collections.Generic;

namespace MBOptionScreen.Interfaces
{
    public interface ISettingsStorage
    {
        List<SettingsBase> AllSettings { get; }
        int SettingsCount { get; }

        List<ModSettingsDefinition> ModSettingsVMs { get; }

        bool RegisterSettings(SettingsBase settingsClass);
        ISerializableFile? GetSettings(string uniqueId);
        void SaveSettings(SettingsBase settingsInstance);
        bool OverrideSettingsWithId(SettingsBase settings, string Id);
        SettingsBase ResetSettingsInstance(SettingsBase settingsInstance);
    }
}