using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Extensions;

using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;
using MCM.Extensions;
using MCM.Implementation.FluentBuilder;
using MCM.Implementation.Settings.Containers.Global;
using MCM.Implementation.Settings.Containers.PerCampaign;
using MCM.Implementation.Settings.Containers.PerSave;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Json2;
using MCM.Implementation.Settings.Formats.Xml;
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

                services.AddSettingsFormat<IJsonSettingsFormat, JsonSettingsFormat>();
                services.AddSettingsFormat<IJson2SettingsFormat, Json2SettingsFormat>();
                services.AddSettingsFormat<IXmlSettingsFormat, XmlSettingsFormat>();

                services.AddSettingsPropertyDiscoverer<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscoverer>();

                services.AddSettingsBuilderFactory<DefaultSettingsBuilderFactory>();

                services.AddSettingsProvider<DefaultSettingsProvider>();

                services.AddScoped<PerSaveCampaignBehavior>();
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

                var settingsProvider = GenericServiceProvider.GetService<BaseSettingsProvider>();
                settingsProvider?.OnGameStarted(game);

                CampaignGameStarter gameStarter = (CampaignGameStarter) gameStarterObject;
                gameStarter.AddBehavior(GenericServiceProvider.GetService<PerSaveCampaignBehavior>());
            }
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            if (game.GameType is Campaign)
            {
                var settingsProvider = GenericServiceProvider.GetService<BaseSettingsProvider>();
                settingsProvider?.OnGameEnded(game);
            }
        }
    }
}