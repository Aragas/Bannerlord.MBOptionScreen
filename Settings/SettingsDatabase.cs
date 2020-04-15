using MBOptionScreen.Interfaces;

using System.Collections.Generic;

namespace MBOptionScreen.Settings
{
    internal static class SettingsDatabase
    {
        private static List<ModSettingsDefinition> _modSettings = null;

        private static ISettingsStorage SettingsStorage => MBOptionScreenSubModule.SharedStateObject.SettingsStorage;

        public static List<SettingsBase> AllSettings => SettingsStorage.AllSettings;
        public static int SettingsCount => SettingsStorage.SettingsCount;
        public static List<ModSettingsDefinition> ModSettingsVMs => _modSettings ??= SettingsStorage.ModSettingsVMs;

        public static bool RegisterSettings(SettingsBase settingsClass) => SettingsStorage.RegisterSettings(settingsClass);

        public static ISerializableFile? GetSettings(string uniqueId) => SettingsStorage.GetSettings(uniqueId);

        public static void SaveSettings(SettingsBase settingsInstance) => SettingsStorage.SaveSettings(settingsInstance);

        public static bool OverrideSettingsWithId(SettingsBase settings, string Id) => SettingsStorage.OverrideSettingsWithId(settings, Id);

        public static SettingsBase ResetSettingsInstance(SettingsBase settingsInstance) => SettingsStorage.ResetSettingsInstance(settingsInstance);
    }
}