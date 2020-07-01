using MCM.Abstractions;
using MCM.Abstractions.Settings.Containers.Global;

namespace MCM.Implementation.ModLib.Settings.Containers.v13
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IModLibDefinitionsSettingsContainer : IGlobalSettingsContainer, IDependency { }
}