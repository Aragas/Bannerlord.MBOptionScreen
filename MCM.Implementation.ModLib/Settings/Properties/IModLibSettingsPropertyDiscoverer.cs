using MCM.Abstractions;
using MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.ModLib.Settings.Properties
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IModLibSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer, IDependency
    {

    }
}