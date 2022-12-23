using System;
using System.Collections.Generic;

namespace MCM.Abstractions.Base
{
    /// <summary>
    /// Interface that declares that the settings a fluent dynamic settings
    /// </summary>
    [Obsolete("Will be internal in the future")]
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IFluentSettings
    {
        List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }
    }
}