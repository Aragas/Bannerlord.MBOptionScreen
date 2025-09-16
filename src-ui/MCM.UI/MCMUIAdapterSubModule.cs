using BUTR.DependencyInjection;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Internal.Extensions;
using MCM.UI.Adapter.MCMv5.Properties;
using MCM.UI.Adapter.MCMv5.Providers;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class MCMUIAdapterSubModule : MBSubModuleBase
    {
        private static void OnAfterSetInitialModuleScreenAsRootScreen()
        {
            var enumerable = GenericServiceProvider.GetService<IEnumerable<IExternalSettingsProvider>>()?.OfType<IExternalSettingsProviderHasInitialize>() ?? [];

            foreach (var hasInitialize in enumerable)
                hasInitialize.Initialize();
        }

        private Harmony Harmony { get; } = new("MCM.UI.Adapter.MCMv5");
        private bool ServiceRegistrationWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsPropertyDiscoverer<MCMv5AttributeSettingsPropertyDiscoverer>();
                services.AddSettingsPropertyDiscoverer<MCMv5FluentSettingsPropertyDiscoverer>();

                services.AddExternalSettingsProvider<MCMv5ExternalSettingsProvider>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();

            Harmony.Patch(
                AccessTools2.Method(typeof(Module), "SetInitialModuleScreenAsRootScreen"),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(MCMUIAdapterSubModule), nameof(OnAfterSetInitialModuleScreenAsRootScreen))));
        }

        /// <inheritdoc />
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            Harmony.Unpatch(
                AccessTools2.Method(typeof(Module), "SetInitialModuleScreenAsRootScreen"),
                AccessTools2.Method(typeof(MCMUIAdapterSubModule), nameof(OnAfterSetInitialModuleScreenAsRootScreen)));
        }
    }
}