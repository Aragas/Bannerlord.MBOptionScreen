using MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.Settings.Properties
{
    public class MCMSettingsPropertyDiscovererWrapper : BaseSettingsPropertyDiscovererWrapper, IMCMSettingsPropertyDiscoverer
    {
        public MCMSettingsPropertyDiscovererWrapper(object @object) : base(@object) { }
    }
}