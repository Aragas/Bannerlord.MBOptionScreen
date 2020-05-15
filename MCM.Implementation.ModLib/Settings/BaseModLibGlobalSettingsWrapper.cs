using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.ModLib.Settings.Properties;
using MCM.Utils;

namespace MCM.Implementation.ModLib.Settings
{
    public abstract class BaseModLibGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IModLibSettingsPropertyDiscoverer, ModLibSettingsPropertyDiscovererWrapper>();

        protected BaseModLibGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}