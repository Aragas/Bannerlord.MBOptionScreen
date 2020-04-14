using ModLib;
using ModLib.GUI.ViewModels;
using ModLib.Interfaces;

using System.Collections.Generic;

namespace MBOptionScreen
{
    public interface ISettingsStorage
    {
        List<SettingsBase> AllSettings { get; }
        int SettingsCount { get; }

        List<ModSettingsVM> ModSettingsVMs { get; }

        bool RegisterSettings(SettingsBase settingsClass);
        ISerialisableFile? GetSettings(string uniqueId);
        void SaveSettings(SettingsBase settingsInstance);
        bool OverrideSettingsWithId(SettingsBase settings, string Id);
        SettingsBase ResetSettingsInstance(SettingsBase settingsInstance);
    }
}