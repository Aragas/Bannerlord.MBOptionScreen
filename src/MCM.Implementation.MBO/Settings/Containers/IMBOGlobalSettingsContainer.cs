using MCM.Abstractions;
using MCM.Abstractions.Settings.Containers.Global;

namespace MCM.Implementation.MBO.Settings.Containers
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMBOGlobalSettingsContainer : IGlobalSettingsContainer, IDependency
    {

    }
}