extern alias v3;
extern alias v4;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Adapter.MCMv3.Patches;
using MCM.Adapter.MCMv3.Settings.Containers;
using MCM.Adapter.MCMv3.Settings.Properties;
using MCM.Adapter.MCMv3.Settings.Providers;

using TaleWorlds.MountAndBlade;

using v4::BUTR.DependencyInjection.Extensions;
using v4::MCM.Extensions;

using MCMv3BaseSettingsProvider = v3::MCM.Abstractions.Settings.Providers.BaseSettingsProvider;

namespace MCM.Adapter.MCMv3
{
    public class MCMMCMv3SubModule : MBSubModuleBase
    {
        private static readonly AccessTools.FieldRef<MCMv3BaseSettingsProvider>? Instance =
            AccessTools2.StaticFieldRefAccess<MCMv3BaseSettingsProvider>(typeof(MCMv3BaseSettingsProvider), "_instance");

        private bool ServiceRegistrationWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsContainer<MCMv3FluentGlobalSettingsContainer>();
                services.AddSettingsContainer<MCMv3GlobalSettingsContainer>();
                services.AddSettingsPropertyDiscoverer<MCMv3SettingsPropertyDiscoverer>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();

            var harmony = new Harmony("bannerlord.mcm.implementation.mcmv3.loaderpreventer");
            MCMv3IntegratedLoaderSubModulePatch.Patch(harmony);

            // Prevents the Settings.Instance exception
            if (Instance is not null)
                Instance() = new MCMv3SettingsProviderReplacer();
        }
    }
}