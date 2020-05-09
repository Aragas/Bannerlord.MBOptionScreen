using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Properties
{
    public interface ISettingsPropertyDiscoverer
    {
        IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object, string id);
    }
}