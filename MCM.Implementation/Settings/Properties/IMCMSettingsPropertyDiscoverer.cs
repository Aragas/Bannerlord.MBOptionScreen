using MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.Settings.Properties
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    {

    }
}