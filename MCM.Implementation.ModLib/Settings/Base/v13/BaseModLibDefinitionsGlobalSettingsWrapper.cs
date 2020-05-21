using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.ModLib.Settings.Properties.v13;
using MCM.Utils;

namespace MCM.Implementation.ModLib.Settings.Base.v13
{
    public abstract class BaseModLibDefinitionsGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IModLibDefinitionsSettingsPropertyDiscoverer, ModLibDefinitionsSettingsPropertyDiscovererWrapper>();

        protected BaseModLibDefinitionsGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}