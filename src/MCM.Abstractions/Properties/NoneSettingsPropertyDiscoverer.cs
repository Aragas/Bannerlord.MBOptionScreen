using MCM.Abstractions.Base;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Properties
{
    public sealed class NoneSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "none" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings) => Enumerable.Empty<ISettingsPropertyDefinition>();
    }
}