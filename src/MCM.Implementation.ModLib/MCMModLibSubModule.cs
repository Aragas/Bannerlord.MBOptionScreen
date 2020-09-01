using Bannerlord.ButterLib.Common.Extensions;

using MCM.Extensions;
using MCM.Implementation.ModLib.Functionality;
using MCM.Implementation.ModLib.Settings.Containers.v1;
using MCM.Implementation.ModLib.Settings.Containers.v13;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.ModLib
{
    public sealed class MCMModLibSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();

            services.AddTransient<BaseModLibScreenOverrider, DefaultModLibScreenOverrider>();

            services.AddSettingsContainer<ModLibSettingsContainer>();
            services.AddSettingsContainer<ModLibDefinitionsSettingsContainer>();

            //services.AddSettingsPropertyDiscoverer<ModLibSettingsPropertyDiscoverer>();
            //services.AddSettingsPropertyDiscoverer<ModLibDefinitionsSettingsPropertyDiscoverer>();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (MCMModLibSettings.Instance!.OverrideModLib)
                BaseModLibScreenOverrider.Instance.OverrideModLibScreen();
        }
    }
}