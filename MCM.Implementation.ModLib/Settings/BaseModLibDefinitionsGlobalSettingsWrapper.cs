using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.ModLib.Settings.Properties;
using MCM.Utils;

namespace MCM.Implementation.ModLib.Settings
{
    public abstract class BaseModLibDefinitionsGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IModLibDefinitionsSettingsPropertyDiscoverer, ModLibDefinitionsSettingsPropertyDiscovererWrapper>();

        protected BaseModLibDefinitionsGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}