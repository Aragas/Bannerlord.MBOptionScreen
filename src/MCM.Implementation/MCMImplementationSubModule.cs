using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Functionality;
using MCM.Abstractions.Settings.Properties;
using MCM.Abstractions.Settings.Providers;
using MCM.Extensions;
using MCM.Implementation.Functionality;
using MCM.Implementation.Settings.Base.Global;
using MCM.Implementation.Settings.Base.PerCampaign;
using MCM.Implementation.Settings.Containers.Global;
using MCM.Implementation.Settings.Containers.PerCampaign;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Xml;
using MCM.Implementation.Settings.Properties;
using MCM.Implementation.Settings.Providers;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation
{
    public sealed class MCMImplementationSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();

            services.AddTransient<BaseGameMenuScreenHandler, DefaultGameMenuScreenHandler>();
            services.AddTransient<BaseIngameMenuScreenHandler, DefaultIngameMenuScreenHandler>();

            services.AddSettingsContainerWrapper<BaseMCMGlobalSettingsWrapper, MCMGlobalSettingsWrapper>();
            services.AddSettingsContainerWrapper<BaseMCMPerCampaignSettingsWrapper, MCMPerCampaignSettingsWrapper>();

            services.AddSettingsContainer<FluentGlobalSettingsContainer>();
            services.AddSettingsContainer<IMCMGlobalSettingsContainer, MCMGlobalSettingsContainer>();
            services.AddSettingsContainer<FluentPerCampaignSettingsContainer>();
            services.AddSettingsContainer<IMCMPerCampaignSettingsContainer, MCMPerCampaignSettingsContainer>();

            services.AddSettingsFormat<IJsonSettingsFormat, JsonSettingsFormat>();
            services.AddSettingsFormat<IXmlSettingsFormat, XmlSettingsFormat>();

            services.AddSettingsPropertyDiscoverer<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscoverer>();

            services.AddSettingsProvider<DefaultSettingsProvider>();
        }


        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);
            BaseSettingsProvider.Instance.OnGameStarted(game);
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            BaseSettingsProvider.Instance.OnGameEnded(game);
        }
    }
}