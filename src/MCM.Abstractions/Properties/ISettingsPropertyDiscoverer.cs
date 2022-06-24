using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Properties
{
    public interface ISettingsPropertyDiscoverer
    {
        IEnumerable<string> DiscoveryTypes { get; }
        IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings);
    }
}