extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.Adapter.MCMv3.Patches;
using MCM.Adapter.MCMv3.Settings.Containers;
using MCM.Adapter.MCMv3.Settings.Properties;
using MCM.Adapter.MCMv3.Settings.Providers;

using TaleWorlds.MountAndBlade;

using v4::MCM.Extensions;

using MCMv3BaseSettingsProvider = v3::MCM.Abstractions.Settings.Providers.BaseSettingsProvider;

namespace MCM.Adapter.MCMv3
{
    public class MCMMCMv3SubModule : MBSubModuleBase
    {
        private static readonly AccessTools.FieldRef<MCMv3BaseSettingsProvider>? Instance =
            AccessTools2.StaticFieldRefAccess<MCMv3BaseSettingsProvider>(AccessTools.Field(typeof(MCMv3BaseSettingsProvider), "_instance"));

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (this.GetServices() is { } services)
            {
                services.AddSettingsContainer<MCMv3FluentGlobalSettingsContainer>();
                services.AddSettingsContainer<MCMv3GlobalSettingsContainer>();
                services.AddSettingsPropertyDiscoverer<MCMv3SettingsPropertyDiscoverer>();
            }

            var harmony = new Harmony("bannerlord.mcm.implementation.mcmv3.loaderpreventer");
            MCMv3IntegratedLoaderSubModulePatch.Patch(harmony);

            // Prevents the Settings.Instance exception
            if (Instance is { })
                Instance() = new MCMv3SettingsProviderReplacer();
        }
    }
}