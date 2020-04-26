using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
{
    internal static class SettingsDatabase
    {
        internal static IModLibSettingsProvider? ModLibSettingsProvider { get; set; }
        internal static IMBOptionScreenSettingsProvider MBOptionScreenSettingsProvider { get; set; } = default!;

        public static List<ModSettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                return MBOptionScreenSettingsProvider.CreateModSettingsDefinitions
                    .Concat(ModLibSettingsProvider?.CreateModSettingsDefinitions ?? new List<ModSettingsDefinition>())
                    .ToList();
            }
        }

        public static bool RegisterSettings(SettingsBase settings)
        {
            if (settings is ModLibSettingsWrapper modLibSettings && ModLibSettingsProvider != null)
                return ModLibSettingsProvider.RegisterSettings(modLibSettings);

            return MBOptionScreenSettingsProvider.RegisterSettings(settings);
        }

        public static SettingsBase? GetSettings(string id)
        {
            return MBOptionScreenSettingsProvider.GetSettings(id)
                ?? ModLibSettingsProvider?.GetSettings(id);
        }

        public static void SaveSettings(SettingsBase settings)
        {
            if (settings is ModLibSettingsWrapper modLibSettings && ModLibSettingsProvider != null)
                ModLibSettingsProvider.SaveSettings(modLibSettings);

            MBOptionScreenSettingsProvider.SaveSettings(settings);
        }

        public static bool OverrideSettings(SettingsBase settings)
        {
            if (settings is ModLibSettingsWrapper modLibSettings && ModLibSettingsProvider != null)
                return ModLibSettingsProvider.OverrideSettings(modLibSettings);

            return MBOptionScreenSettingsProvider.OverrideSettings(settings);
        }

        public static SettingsBase ResetSettings(string id)
        {
            return MBOptionScreenSettingsProvider.ResetSettings(id)
                ?? ModLibSettingsProvider?.ResetSettings(id);
        }
    }
}