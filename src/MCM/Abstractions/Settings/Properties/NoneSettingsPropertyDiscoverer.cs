using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Settings.Properties
{
    public sealed class NoneSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "none" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings) => Enumerable.Empty<ISettingsPropertyDefinition>();
    }
}