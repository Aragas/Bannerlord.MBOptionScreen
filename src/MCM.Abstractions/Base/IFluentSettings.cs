using MCM.Abstractions.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Base
{
    /// <summary>
    /// Tnterface that declares that the settings a fluent dynamic settings
    /// </summary>
    public interface IFluentSettings
    {
        List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }
    }
}