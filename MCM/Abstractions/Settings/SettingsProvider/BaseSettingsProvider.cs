using MCM.Abstractions.Settings.Definitions;
using MCM.Utils;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.SettingsProvider
{
    public abstract class BaseSettingsProvider
    {
        private static BaseSettingsProvider? _instance;
        public static BaseSettingsProvider Instance => _instance
            ?? (_instance = DI.GetImplementation<BaseSettingsProvider, SettingsProviderWrapper>(ApplicationVersionUtils.GameVersion()));

        public abstract IEnumerable<SettingsDefinition> CreateModSettingsDefinitions { get; }
        public abstract SettingsBase? GetSettings(string id);
        public abstract void RegisterSettings(SettingsBase setting);
        public abstract void SaveSettings(SettingsBase settings);
        public abstract SettingsBase? ResetSettings(string idd);
        public abstract void OverrideSettings(SettingsBase settings);
    }
}