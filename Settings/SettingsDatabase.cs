using MBOptionScreen.Interfaces;

using System.Collections.Generic;

namespace MBOptionScreen.Settings
{
    internal static class SettingsDatabase
    {
        private static ISettingsProvider SettingsStorage => MBOptionScreenSubModule.SharedStateObject.SettingsStorage;

        public static List<ModSettingsDefinition> CreateModSettingsDefinitions => SettingsStorage.CreateModSettingsDefinitions;

        public static bool RegisterSettings(SettingsBase settingsClass) => SettingsStorage.RegisterSettings(settingsClass);

        public static SettingsBase? GetSettings(string uniqueId) => SettingsStorage.GetSettings(uniqueId);

        public static void SaveSettings(SettingsBase settingsInstance) => SettingsStorage.SaveSettings(settingsInstance);

        public static bool OverrideSettingsWithId(SettingsBase settings, string Id) => SettingsStorage.OverrideSettingsWithId(settings, Id);

        public static SettingsBase ResetSettingsInstance(SettingsBase settingsInstance) => SettingsStorage.ResetSettingsInstance(settingsInstance);
    }
}