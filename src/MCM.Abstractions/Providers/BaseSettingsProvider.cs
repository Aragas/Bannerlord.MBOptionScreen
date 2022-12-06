using BUTR.DependencyInjection;

using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
    /// <summary>
    /// The interface that is responsible for providing and manipulating settings
    /// for the end user - modder
    /// </summary>
    public abstract class BaseSettingsProvider
    {
        public static BaseSettingsProvider? Instance { get; } = GenericServiceProvider.GetService<BaseSettingsProvider>();

        public abstract IEnumerable<SettingsDefinition> SettingsDefinitions { get; }
        public abstract BaseSettings? GetSettings(string id);
        public abstract void SaveSettings(BaseSettings settings);
        public abstract void ResetSettings(BaseSettings settings);
        public abstract void OverrideSettings(BaseSettings settings);
        public abstract IEnumerable<ISettingsPreset> GetPresets(string id);
    }
}