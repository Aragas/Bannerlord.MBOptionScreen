using MCM.Abstractions.Settings.Properties;

namespace MCM.Implementation.ModLib.Settings.Properties
{
    /// <summary>
    /// For DI
    /// </summary>
    public class ModLibSettingsPropertyDiscovererWrapper : BaseSettingsPropertyDiscovererWrapper, IModLibSettingsPropertyDiscoverer
    {
        public ModLibSettingsPropertyDiscovererWrapper(object @object) : base(@object) { }
    }
}