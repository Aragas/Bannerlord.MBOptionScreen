extern alias v4;

using v4::MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.MCMv3.Settings.Properties
{
    /// <summary>
    /// For DI
    /// </summary>
    public class MCMv3SettingsPropertyDiscovererWrapper : BaseSettingsPropertyDiscovererWrapper, IMCMv3SettingsPropertyDiscoverer
    {
        public MCMv3SettingsPropertyDiscovererWrapper(object @object) : base(@object) { }
    }
}