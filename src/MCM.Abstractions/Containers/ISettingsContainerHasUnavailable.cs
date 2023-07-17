using System.Collections.Generic;

namespace MCM.Abstractions
{
    /// <summary>
    /// Interface that lists unavailable settings in the global menu
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        interface ISettingsContainerHasUnavailable
    {
        IEnumerable<UnavailableSetting> GetUnavailableSettings();
    }
}