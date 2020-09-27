using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Base
{
    public interface IFluentSettings
    {
        List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }
    }
}