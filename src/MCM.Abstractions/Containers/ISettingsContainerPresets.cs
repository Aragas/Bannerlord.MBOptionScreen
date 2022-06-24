using MCM.Abstractions.Settings.Presets;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Containers
{
    public interface ISettingsContainerPresets
    {
        IEnumerable<ISettingsPreset> GetPresets(string settingsId);
        bool SavePresets(ISettingsPreset preset);
    }
}