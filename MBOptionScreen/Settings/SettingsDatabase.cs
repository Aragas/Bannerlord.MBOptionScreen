using MBOptionScreen.Interfaces;
using MBOptionScreen.Settings.Wrapper;

using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
{
    internal static class SettingsDatabase
    {
        private static ISettingsProvider ModLibSettingsProvider = new ModLibSettingsProviderWrapper();
        private static ISettingsProvider SettingsProvider => MBOptionScreenSubModule.SharedStateObject.SettingsStorage;

        public static List<ModSettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                return SettingsProvider.CreateModSettingsDefinitions
                    .Concat(ModLibSettingsProvider.CreateModSettingsDefinitions)
                    .ToList();
            }
        }

        public static bool RegisterSettings(SettingsBase settings)
        {
            if (settings is ModLibSettingsWrapper modLibSettings)
                return ModLibSettingsProvider.RegisterSettings(modLibSettings);

            return SettingsProvider.RegisterSettings(settings);
        }

        public static SettingsBase? GetSettings(string id)
        {
            return SettingsProvider.GetSettings(id)
                ?? ModLibSettingsProvider.GetSettings(id);
        }

        public static void SaveSettings(SettingsBase settings)
        {
            if (settings is ModLibSettingsWrapper modLibSettings)
                ModLibSettingsProvider.SaveSettings(modLibSettings);

            SettingsProvider.SaveSettings(settings);
        }

        public static bool OverrideSettings(SettingsBase settings)
        {
            if (settings is ModLibSettingsWrapper modLibSettings)
                return ModLibSettingsProvider.OverrideSettings(modLibSettings);

            return SettingsProvider.OverrideSettings(settings);
        }

        public static SettingsBase ResetSettings(string id)
        {
            return SettingsProvider.ResetSettings(id)
                ?? ModLibSettingsProvider.ResetSettings(id);
        }
    }
}