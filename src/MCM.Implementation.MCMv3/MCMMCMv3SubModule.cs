extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using MCM.Implementation.MCMv3.Settings.Containers;
using MCM.Implementation.MCMv3.Settings.Properties;
using MCM.Implementation.MCMv3.Settings.Providers;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

using TaleWorlds.MountAndBlade;

using v4::MCM.Abstractions.Settings.Providers;
using v4::MCM.Extensions;

using MCMv3BaseSettingsProvider = v3::MCM.Abstractions.Settings.Providers.BaseSettingsProvider;

namespace MCM.Implementation.MCMv3
{
    public class MCMMCMv3SubModule : MBSubModuleBase
    {
        // Prevent any Integrated MCMv3 from loading
        private class EmptyIntegratedLoader : v3::MCM.Abstractions.Loader.IIntegratedLoader
        {
            public List<(MBSubModuleBase, System.Type)> MCMImplementationSubModules { get; } = new List<(MBSubModuleBase, System.Type)>();

            public void Load() { }
        }
        private static AccessTools.FieldRef<v3::MCM.IntegratedLoaderSubModule, v3::MCM.Abstractions.Loader.IIntegratedLoader> _loader =
            AccessTools.FieldRefAccess<v3::MCM.IntegratedLoaderSubModule, v3::MCM.Abstractions.Loader.IIntegratedLoader>("_loader");
        private static bool StopIntegratedLoaderSubModuleCtor(v3::MCM.IntegratedLoaderSubModule __instance)
        {
            _loader(__instance) = new EmptyIntegratedLoader();

            return false;
        }
        // Prevent any Integrated MCMv3 from loading

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();

            services.AddSettingsContainer<IMCMv3FluentGlobalSettingsContainer, MCMv3FluentGlobalSettingsContainer>();
            services.AddSettingsContainer<IMCMv3GlobalSettingsContainer, MCMv3GlobalSettingsContainer>();
            services.AddSettingsPropertyDiscoverer<IMCMv3SettingsPropertyDiscoverer, MCMv3SettingsPropertyDiscoverer>();

            var harmony = new Harmony("bannerlord.mcm.implementation.mcmv3.loaderpreventer");
            harmony.Patch(AccessTools.Constructor(typeof(v3::MCM.IntegratedLoaderSubModule)),
                prefix: new HarmonyMethod(SymbolExtensions.GetMethodInfo(() => StopIntegratedLoaderSubModuleCtor(null!))));

            var field = AccessTools.Field(typeof(MCMv3BaseSettingsProvider), "_instance");
            field.SetValue(null, new MCMv3SettingsProviderReplacer());
        }
    }
}