using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Settings.Properties;
using MCM.Extensions;
using MCM.Implementation.FluentBuilder;
using MCM.Implementation.Settings.Containers.Global;
using MCM.Implementation.Settings.Containers.PerCampaign;
using MCM.Implementation.Settings.Containers.PerSave;
using MCM.Implementation.Settings.Formats;
using MCM.Implementation.Settings.Properties;
using MCM.Implementation.Settings.Providers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation
{
    public sealed class MCMImplementationSubModule : MBSubModuleBase
    {
        private bool ServiceRegistrationWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsContainer<IMCMFluentGlobalSettingsContainer, FluentGlobalSettingsContainer>();
                services.AddSettingsContainer<IMCMGlobalSettingsContainer, MCMGlobalSettingsContainer>();
                services.AddSettingsContainer<IMCMFluentPerSaveSettingsContainer, FluentPerSaveSettingsContainer>();
                services.AddSettingsContainer<IMCMPerSaveSettingsContainer, MCMPerSaveSettingsContainer>();
                services.AddSettingsContainer<IMCMFluentPerCampaignSettingsContainer, FluentPerCampaignSettingsContainer>();
                services.AddSettingsContainer<IMCMPerCampaignSettingsContainer, MCMPerCampaignSettingsContainer>();

                services.AddSettingsFormat<JsonSettingsFormat>();
                services.AddSettingsFormat<XmlSettingsFormat>();

                services.AddSettingsPropertyDiscoverer<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscoverer>();

                services.AddSettingsBuilderFactory<DefaultSettingsBuilderFactory>();

                services.AddSettingsProvider<DefaultSettingsProvider>();

                services.AddScoped<PerSaveCampaignBehavior>();
                services.RegisterScoped<IGameEventListener, GameEventListener>();
                //services.AddScoped<IGameEventListener, GameEventListener>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();
        }


        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                /*
                CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, _ =>
                {
                    var settingsProvider = this.GetServiceProvider()?.GetRequiredService<BaseSettingsProvider>();
                    settingsProvider?.OnGameStarted(game);
                });
                CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, _ =>
                {
                    var settingsProvider = this.GetServiceProvider()?.GetRequiredService<BaseSettingsProvider>();
                    settingsProvider?.OnGameStarted(game);
                });
                */

                if (GenericServiceProvider.GetService<IGameEventListener>() is GameEventListener gameEventListener)
                    gameEventListener.GameStarted(game);

                var gameStarter = (CampaignGameStarter) gameStarterObject;
                gameStarter.AddBehavior(GenericServiceProvider.GetService<PerSaveCampaignBehavior>());
            }
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            if (game.GameType is Campaign)
            {
                if (GenericServiceProvider.GetService<IGameEventListener>() is GameEventListener gameEventListener)
                    gameEventListener.GameEnded(game);
            }
        }
    }
}