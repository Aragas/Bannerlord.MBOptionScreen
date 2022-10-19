using Bannerlord.BUTR.Shared.Helpers;

using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Extensions;
using BUTR.DependencyInjection.LightInject;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;
using MCM.Extensions;
using MCM.LightInject;
using MCM.Utils;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using ServiceCollectionExtensions = BUTR.DependencyInjection.Extensions.ServiceCollectionExtensions;

namespace MCM
{
    public sealed class MCMSubModule : MBSubModuleBase
    {
        internal static IBUTRLogger<MCMSubModule> Logger = new DefaultBUTRLogger<MCMSubModule>();

        internal static ServiceContainer LightInjectServiceContainer = new();

        public static MCMSubModule? Instance { get; private set; }

        private bool ServiceRegistrationWasCalled { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }

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

                services.AddTransient<IBUTRLogger, DefaultBUTRLogger>();
                services.AddTransient(typeof(IBUTRLogger<>), typeof(DefaultBUTRLogger<>));
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();

            if (ApplicationVersionHelper.GameVersion() is { } gameVersion)
            {
                if (gameVersion.Major is 1 && gameVersion.Minor is 8 && gameVersion.Revision is >= 0)
                {
                    LocalizedTextManagerHelper.LoadLanguageData(ModuleInfoHelper.GetModuleByType(typeof(MCMSubModule)));
                }
            }
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            Instance = null;
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!OnBeforeInitialModuleScreenSetAsRootWasCalled)
            {
                OnBeforeInitialModuleScreenSetAsRootWasCalled = true;

                GenericServiceProvider.ServiceProvider = ServiceCollectionExtensions.ServiceContainer.Build();
                Logger = GenericServiceProvider.ServiceProvider.GetService<IBUTRLogger<MCMSubModule>>() ?? Logger;
            }
        }

        public void OverrideServiceContainer(IGenericServiceContainer serviceContainer)
        {
            if (ServiceCollectionExtensions.ServiceContainer is { } oldServiceContainer)
            {
                ServiceCollectionExtensions.ServiceContainer = new WithHistoryGenericServiceContainer(serviceContainer);
                foreach (var historyAction in oldServiceContainer.History)
                {
                    historyAction(ServiceCollectionExtensions.ServiceContainer);
                }
            }
            else
            {
                ServiceCollectionExtensions.ServiceContainer = new WithHistoryGenericServiceContainer(serviceContainer);
            }
        }


        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);

            if (starterObject is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(new SettingsProviderCampaignBehavior(GenericServiceProvider.GetService<BaseSettingsProvider>()));
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            mission.AddMissionBehavior(new SettingsProviderMissionBehavior(GenericServiceProvider.GetService<BaseSettingsProvider>()));
        }
    }
}