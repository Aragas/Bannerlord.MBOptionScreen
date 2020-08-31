extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using MCM.Implementation.MCMv3.Patches;
using MCM.Implementation.MCMv3.Settings.Containers;
using MCM.Implementation.MCMv3.Settings.Providers;

using TaleWorlds.MountAndBlade;

using v4::MCM.Extensions;

using MCMv3BaseSettingsProvider = v3::MCM.Abstractions.Settings.Providers.BaseSettingsProvider;

namespace MCM.Implementation.MCMv3
{
    public class MCMMCMv3SubModule : MBSubModuleBase
    {
        private static readonly AccessTools.FieldRef<MCMv3BaseSettingsProvider, MCMv3BaseSettingsProvider> Instance =
            AccessTools.FieldRefAccess<MCMv3BaseSettingsProvider, MCMv3BaseSettingsProvider>("_instance");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();

            services.AddSettingsContainer<MCMv3FluentGlobalSettingsContainer>();
            services.AddSettingsContainer<MCMv3GlobalSettingsContainer>();
            //services.AddSettingsPropertyDiscoverer<MCMv3SettingsPropertyDiscoverer>();

            var harmony = new Harmony("bannerlord.mcm.implementation.mcmv3.loaderpreventer");
            MCMv3IntegratedLoaderSubModulePatch.Patch(harmony);

            Instance() = new MCMv3SettingsProviderReplacer();
        }
    }
}