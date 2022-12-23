using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Properties
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyDiscoverer
    {
        IEnumerable<string> DiscoveryTypes { get; }
        IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings);
    }
}