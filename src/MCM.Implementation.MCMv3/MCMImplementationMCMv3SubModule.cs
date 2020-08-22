extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Implementation.MCMv3.Settings.Base;
using MCM.Implementation.MCMv3.Settings.Containers;
using MCM.Implementation.MCMv3.Settings.Properties;

using TaleWorlds.MountAndBlade;

using v4::MCM.Extensions;

namespace MCM.Implementation.MCMv3
{
    public class MCMImplementationMCMv3SubModule : MBSubModuleBase
    {
        private readonly v3::MCM.IntegratedLoaderSubModule? _ignore; // Have a reference at v3

        public MCMImplementationMCMv3SubModule()
        {
            _ignore = null;
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();
            services.AddSettingsContainerWrapper<BaseMCMv3GlobalSettingsWrapper, MCMv3GlobalSettingsWrapper>();
            services.AddSettingsContainer<IMCMv3GlobalSettingsContainer, MCMv3GlobalSettingsContainer>();
            services.AddSettingsPropertyDiscoverer<IMCMv3SettingsPropertyDiscoverer, MCMv3SettingsPropertyDiscoverer>();
        }
    }
}