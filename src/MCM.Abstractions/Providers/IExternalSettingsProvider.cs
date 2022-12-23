using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
    /// <summary>
    /// Used to add foreign Options API's that MCM will be able to use.
    /// Most likely will be used to ease backwards compatibility ports of older MCM API's so we'll be able to reuse more code.
    /// This is a higher level alternative to using <see cref="ISettingsContainer"/>.
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IExternalSettingsProvider
    {
        IEnumerable<SettingsDefinition> SettingsDefinitions { get; }
        BaseSettings? GetSettings(string id);
        void SaveSettings(BaseSettings settings);
        void ResetSettings(BaseSettings settings);
        void OverrideSettings(BaseSettings settings);
        IEnumerable<ISettingsPreset> GetPresets(string id);
    }
}