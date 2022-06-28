using System.Collections.Generic;

namespace MCM.Abstractions
{
    /// <summary>
    /// Interface that declares that the <see cref="ISettingsContainer"/> provides <see cref="SettingsDefinition"/>
    /// </summary>
    public interface ISettingsContainerHasSettingsDefinitions
    {
        IEnumerable<SettingsDefinition> SettingsDefinitions { get; }
    }
}