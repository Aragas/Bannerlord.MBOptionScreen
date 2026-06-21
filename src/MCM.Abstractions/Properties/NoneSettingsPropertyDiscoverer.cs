using MCM.Abstractions.Base;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Properties
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class NoneSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "none" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings) => [];
    }
}