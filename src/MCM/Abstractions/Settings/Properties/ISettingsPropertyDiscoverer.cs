using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Properties
{
    public interface ISettingsPropertyDiscoverer : IDependencyBase
    {
        IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object);
    }
}