using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.DependencyInjection;
using MCM.Extensions;
using MCM.LightInject;

using TaleWorlds.MountAndBlade;

namespace MCM
{
    public sealed class MCMSubModule : MBSubModuleBase
    {
        internal static ServiceContainer LightInjectServiceContainer = new();

        public static MCMSubModule? Instance { get; private set; }

        private bool ServiceRegistrationWasCalled { get; set; }

        public MCMSubModule()
        {
            Instance = this;

            ServiceCollectionExtensions.ServiceContainer = new WithHistoryGenericServiceContainer(new LightInjectServiceContainer(LightInjectServiceContainer));
        }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsFormat<MemorySettingsFormat>();

                services.AddSettingsFormat<MemorySettingsFormat>();
                services.AddSettingsPropertyDiscoverer<NoneSettingsPropertyDiscoverer>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            Instance = null;
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            GenericServiceProvider.ServiceProvider = ServiceCollectionExtensions.ServiceContainer.Build();
        }

        public void OverrideServiceContainer(IGenericServiceContainer serviceContainer)
        {
            var oldServiceContainer = ServiceCollectionExtensions.ServiceContainer;
            ServiceCollectionExtensions.ServiceContainer = new WithHistoryGenericServiceContainer(serviceContainer);
            foreach (var historyAction in oldServiceContainer.History)
            {
                historyAction(ServiceCollectionExtensions.ServiceContainer);
            }
        }
    }
}