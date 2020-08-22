extern alias v4;

using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Implementation.MCMv3.Settings.Properties;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Properties;
using v4::MCM.Extensions;

namespace MCM.Implementation.MCMv3.Settings.Base
{
    public abstract class BaseMCMv3GlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected override ISettingsPropertyDiscoverer? Discoverer { get; } =
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IMCMv3SettingsPropertyDiscoverer, MCMv3SettingsPropertyDiscovererWrapper>();
            //DI.GetImplementation<IMCMv3SettingsPropertyDiscoverer, MCMv3SettingsPropertyDiscovererWrapper>();

        protected BaseMCMv3GlobalSettingsWrapper(object @object) : base(@object) { }
    }
}