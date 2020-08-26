extern alias v4;

using v4::MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.MCMv3.Settings.Properties
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMv3SettingsPropertyDiscoverer : ISettingsPropertyDiscoverer { }
}