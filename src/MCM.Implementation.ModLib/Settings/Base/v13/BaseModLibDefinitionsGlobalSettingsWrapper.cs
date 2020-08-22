using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Properties;
using MCM.Extensions;
using MCM.Implementation.ModLib.Settings.Properties.v13;

namespace MCM.Implementation.ModLib.Settings.Base.v13
{
    public abstract class BaseModLibDefinitionsGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; } =
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IModLibDefinitionsSettingsPropertyDiscoverer, ModLibDefinitionsSettingsPropertyDiscovererWrapper>();
            //DI.GetImplementation<IModLibDefinitionsSettingsPropertyDiscoverer, ModLibDefinitionsSettingsPropertyDiscovererWrapper>();

        protected BaseModLibDefinitionsGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}