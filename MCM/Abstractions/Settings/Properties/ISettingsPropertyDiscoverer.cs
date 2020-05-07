using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Properties
{
    public interface ISettingsPropertyDiscoverer
    {
        IEnumerable<SettingsPropertyDefinition> GetProperties(object @object, string id);
    }
}