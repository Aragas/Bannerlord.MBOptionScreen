extern alias v4;

using v4::MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.MBO.Settings.Properties
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMBOSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer { }
}