using MCM.Adapter.ModLib.Functionality;
using MCM.Adapter.ModLib.Settings.Containers.v1;
using MCM.Adapter.ModLib.Settings.Containers.v13;
using MCM.Adapter.ModLib.Settings.Properties.v1;
using MCM.Adapter.ModLib.Settings.Properties.v13;
using MCM.Extensions;

using TaleWorlds.MountAndBlade;

namespace MCM.Adapter.ModLib
{
    public sealed class MCMModLibSubModule : MBSubModuleBase
    {
        private bool ServiceRegistrationWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddTransient<BaseModLibScreenOverrider, DefaultModLibScreenOverrider>();

                services.AddSettingsContainer<ModLibSettingsContainer>();
                services.AddSettingsContainer<ModLibDefinitionsSettingsContainer>();

                services.AddSettingsPropertyDiscoverer<ModLibSettingsPropertyDiscoverer>();
                services.AddSettingsPropertyDiscoverer<ModLibDefinitionsSettingsPropertyDiscoverer>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (MCMModLibSettings.Instance!.OverrideModLib)
                BaseModLibScreenOverrider.Instance?.OverrideModLibScreen();
        }
    }
}