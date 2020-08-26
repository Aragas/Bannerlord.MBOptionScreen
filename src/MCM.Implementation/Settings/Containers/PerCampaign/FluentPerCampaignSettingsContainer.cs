using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base.PerCampaign;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.PerCampaign;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCampaign
{
    public class FluentPerCampaignSettingsContainer : BaseSettingsContainer<FluentPerCampaignSettings>, IFluentPerCampaignSettingsContainer
    {
        protected override string RootFolder { get; }

        public override List<SettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                ReloadAll();

                return LoadedSettings.Keys
                    .Select(id => new SettingsDefinition(id))
                    .OrderByDescending(a => a.DisplayName)
                    .ToList();
            }
        }

        public FluentPerCampaignSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "PerCampaign");
        }

        private void ReloadAll()
        {
            if (AppDomain.CurrentDomain.GetData(FluentPerCampaignSettings.ContainerId) == null)
                AppDomain.CurrentDomain.SetData(FluentPerCampaignSettings.ContainerId, new Dictionary<string, FluentPerCampaignSettings>());

            var storage = (AppDomain.CurrentDomain.GetData(FluentPerCampaignSettings.ContainerId) as Dictionary<string, FluentPerCampaignSettings>)!;
            foreach (var (id, settings) in storage)
            {
                if (!LoadedSettings.ContainsKey(id))
                    RegisterSettings(settings);
            }
        }

        public void OnGameStarted(Game game) => LoadedSettings.Clear();
        public void OnGameEnded(Game game) => LoadedSettings.Clear();
    }
}