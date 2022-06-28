using System.Collections.Generic;

namespace MCM.Abstractions
{
    public interface ISettingsContainerPresets
    {
        IEnumerable<ISettingsPreset> GetPresets(string settingsId);
        bool SavePresets(ISettingsPreset preset);
    }
}