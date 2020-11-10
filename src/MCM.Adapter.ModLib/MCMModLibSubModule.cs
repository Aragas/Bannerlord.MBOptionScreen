using Bannerlord.ButterLib.Common.Extensions;

using MCM.Adapter.ModLib.Functionality;
using MCM.Adapter.ModLib.Settings.Containers.v1;
using MCM.Adapter.ModLib.Settings.Containers.v13;
using MCM.Adapter.ModLib.Settings.Properties.v1;
using MCM.Adapter.ModLib.Settings.Properties.v13;
using MCM.Extensions;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.MountAndBlade;

namespace MCM.Adapter.ModLib
{
    public sealed class MCMModLibSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (this.GetServices() is { } services)
            {
                services.AddTransient<BaseModLibScreenOverrider, DefaultModLibScreenOverrider>();

                services.AddSettingsContainer<ModLibSettingsContainer>();
                services.AddSettingsContainer<ModLibDefinitionsSettingsContainer>();

                services.AddSettingsPropertyDiscoverer<ModLibSettingsPropertyDiscoverer>();
                services.AddSettingsPropertyDiscoverer<ModLibDefinitionsSettingsPropertyDiscoverer>();
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (MCMModLibSettings.Instance!.OverrideModLib)
                BaseModLibScreenOverrider.Instance?.OverrideModLibScreen();
        }
    }
}