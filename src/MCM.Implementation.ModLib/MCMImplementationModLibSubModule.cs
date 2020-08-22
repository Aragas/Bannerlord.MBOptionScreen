using Bannerlord.ButterLib.Common.Extensions;

using MCM.Extensions;
using MCM.Implementation.ModLib.Functionality;
using MCM.Implementation.ModLib.Settings.Base.v1;
using MCM.Implementation.ModLib.Settings.Base.v13;
using MCM.Implementation.ModLib.Settings.Containers.v1;
using MCM.Implementation.ModLib.Settings.Containers.v13;
using MCM.Implementation.ModLib.Settings.Properties.v1;
using MCM.Implementation.ModLib.Settings.Properties.v13;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.ModLib
{
    public sealed class MCMImplementationModLibSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();

            services.AddTransient<BaseModLibScreenOverrider, DefaultModLibScreenOverrider>();

            services.AddSettingsContainerWrapper<BaseModLibGlobalSettingsWrapper, ModLibGlobalSettingsWrapper>();
            services.AddSettingsContainerWrapper<BaseModLibDefinitionsGlobalSettingsWrapper, ModLibDefinitionsGlobalSettingsWrapper>();
            
            services.AddSettingsContainer<IModLibSettingsContainer, ModLibSettingsContainer>();
            services.AddSettingsContainer<IModLibDefinitionsSettingsContainer, ModLibDefinitionsSettingsContainer>();

            services.AddSettingsPropertyDiscoverer<IModLibSettingsPropertyDiscoverer, ModLibSettingsPropertyDiscoverer>();
            services.AddSettingsPropertyDiscoverer<IModLibDefinitionsSettingsPropertyDiscoverer, ModLibDefinitionsSettingsPropertyDiscoverer>();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (MCMModLibSettings.Instance!.OverrideModLib)
                BaseModLibScreenOverrider.Instance.OverrideModLibScreen();
        }
    }
}