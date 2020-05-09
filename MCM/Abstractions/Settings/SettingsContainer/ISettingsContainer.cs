using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public interface ISettingsContainer
    {
        List<SettingsDefinition> CreateModSettingsDefinitions { get; }

        BaseSettings? GetSettings(string id);
        bool SaveSettings(BaseSettings settings);
        bool OverrideSettings(BaseSettings settings);
        bool ResetSettings(BaseSettings settings);
    }
}