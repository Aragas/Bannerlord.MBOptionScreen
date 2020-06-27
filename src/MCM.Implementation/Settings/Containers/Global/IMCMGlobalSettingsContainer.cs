using MCM.Abstractions;
using MCM.Abstractions.Settings.Containers.Global;

namespace MCM.Implementation.Settings.Containers.Global
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMGlobalSettingsContainer : IGlobalSettingsContainer, IDependency
    {

    }
}