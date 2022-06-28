using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Properties
{
    public interface ISettingsPropertyDiscoverer
    {
        IEnumerable<string> DiscoveryTypes { get; }
        IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings);
    }
}