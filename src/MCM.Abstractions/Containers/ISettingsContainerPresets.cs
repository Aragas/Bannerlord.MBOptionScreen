using System.Collections.Generic;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsContainerPresets
    {
        IEnumerable<ISettingsPreset> GetPresets(string settingsId);
        bool SavePresets(ISettingsPreset preset);
    }
}