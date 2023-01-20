using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Extensions;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Properties;
using MCM.Internal.Extensions;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using ServiceCollectionExtensions = BUTR.DependencyInjection.Extensions.ServiceCollectionExtensions;

using MCM.LightInject;

namespace MCM
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class MCMSubModule : MBSubModuleBase
    {
        private static IBUTRLogger<MCMSubModule> Logger = new DefaultBUTRLogger<MCMSubModule>();

        private static readonly ServiceContainer LightInjectServiceContainer = new(options =>
        {
            options.EnableCurrentScope = false;
        });

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

                GenericServiceProvider.GlobalServiceProvider = ServiceCollectionExtensions.ServiceContainer.Build();
                Logger = GenericServiceProvider.GetService<IBUTRLogger<MCMSubModule>>() ?? Logger;
                BaseSettingsProvider.Instance = GenericServiceProvider.GetService<BaseSettingsProvider>(); // Force loading
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

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            GenericServiceProvider.GameScopeServiceProvider = GenericServiceProvider.CreateScope();
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            GenericServiceProvider.GameScopeServiceProvider?.Dispose();
            GenericServiceProvider.GameScopeServiceProvider = null;
        }
    }
}