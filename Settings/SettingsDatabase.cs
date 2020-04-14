using MBOptionScreen;
using ModLib.Interfaces;
using System.Collections.Generic;

namespace ModLib
{
    internal static class SettingsDatabase
    {
        private static List<ModSettingsDefinition> _modSettings = null;

        private static ISettingsStorage SettingsStorage => MBOptionScreenSubModule.SharedStateObject.SettingsStorage;

        public static List<SettingsBase> AllSettings => SettingsStorage.AllSettings;
        public static int SettingsCount => SettingsStorage.SettingsCount;
        public static List<ModSettingsDefinition> ModSettingsVMs => _modSettings ??= SettingsStorage.ModSettingsVMs;

        public static bool RegisterSettings(SettingsBase settingsClass) => SettingsStorage.RegisterSettings(settingsClass);

        public static ISerialisableFile? GetSettings(string uniqueId) => SettingsStorage.GetSettings(uniqueId);

        public static void SaveSettings(SettingsBase settingsInstance) => SettingsStorage.SaveSettings(settingsInstance);

        public static bool OverrideSettingsWithID(SettingsBase settings, string ID) => SettingsStorage.OverrideSettingsWithId(settings, ID);

        public static SettingsBase ResetSettingsInstance(SettingsBase settingsInstance) => SettingsStorage.ResetSettingsInstance(settingsInstance);
    }
}