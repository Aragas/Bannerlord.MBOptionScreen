using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Properties;
using MCM.Extensions;
using MCM.Implementation.ModLib.Settings.Properties.v1;

namespace MCM.Implementation.ModLib.Settings.Base.v1
{
    public abstract class BaseModLibGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; } =
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IModLibSettingsPropertyDiscoverer, ModLibSettingsPropertyDiscovererWrapper>();
            //DI.GetImplementation<IModLibSettingsPropertyDiscoverer, ModLibSettingsPropertyDiscovererWrapper>();

        protected BaseModLibGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}