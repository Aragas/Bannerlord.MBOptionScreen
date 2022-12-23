using System.Collections.Generic;

namespace MCM.Abstractions
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> provides <see cref="SettingsDefinition"/>
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsContainerHasSettingsDefinitions
    {
        IEnumerable<SettingsDefinition> SettingsDefinitions { get; }
    }
}