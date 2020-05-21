using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Properties;
using MCM.Implementation.ModLib.Settings.Properties.v1;
using MCM.Utils;

namespace MCM.Implementation.ModLib.Settings.Base.v1
{
    public abstract class BaseModLibGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IModLibSettingsPropertyDiscoverer, ModLibSettingsPropertyDiscovererWrapper>();

        protected BaseModLibGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}