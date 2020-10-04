using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;
using MCM.Extensions;
using MCM.Implementation.FluentBuilder;
using MCM.Implementation.Settings.Containers.Custom;
using MCM.Implementation.Settings.Containers.Global;
using MCM.Implementation.Settings.Containers.PerCampaign;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Json2;
using MCM.Implementation.Settings.Formats.Xml;
using MCM.Implementation.Settings.Properties;
using MCM.Implementation.Settings.Providers;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation
{
    public sealed class MCMImplementationSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (this.GetServices() is { } services)
            {
                services.AddSettingsContainer<IMCMFluentGlobalSettingsContainer, FluentGlobalSettingsContainer>();
                services.AddSettingsContainer<IMCMGlobalSettingsContainer, MCMGlobalSettingsContainer>();
                services.AddSettingsContainer<IMCMFluentPerCampaignSettingsContainer, FluentPerCampaignSettingsContainer>();
                services.AddSettingsContainer<IMCMPerCampaignSettingsContainer, MCMPerCampaignSettingsContainer>();
                services.AddSettingsContainer<ButterLibSettingsContainer>();

                services.AddSettingsFormat<IJsonSettingsFormat, JsonSettingsFormat>();
                services.AddSettingsFormat<IJson2SettingsFormat, Json2SettingsFormat>();
                services.AddSettingsFormat<IXmlSettingsFormat, XmlSettingsFormat>();

                services.AddSettingsPropertyDiscoverer<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscoverer>();

                services.AddSettingsBuilderFactory<DefaultSettingsBuilderFactory>();

                services.AddSettingsProvider<DefaultSettingsProvider>();
            }
        }


        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);
            BaseSettingsProvider.Instance!.OnGameStarted(game);
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            BaseSettingsProvider.Instance!.OnGameEnded(game);
        }
    }
}