using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
    /// <summary>
    /// The interface that is responsible for providing and manipulating settings
    /// for the end user - modder
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class BaseSettingsProvider
    {
        public static BaseSettingsProvider? Instance { get; internal set; }

        public abstract IEnumerable<SettingsDefinition> SettingsDefinitions { get; }
        public abstract BaseSettings? GetSettings(string id);
        public abstract void SaveSettings(BaseSettings settings);
        public abstract void ResetSettings(BaseSettings settings);
        public abstract void OverrideSettings(BaseSettings settings);
        public abstract IEnumerable<ISettingsPreset> GetPresets(string id);
        public abstract IEnumerable<UnavailableSetting> GetUnavailableSettings();
    }
}