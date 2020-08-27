using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Properties
{
    public interface ISettingsPropertyDiscoverer
    {
        IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object);
    }
}