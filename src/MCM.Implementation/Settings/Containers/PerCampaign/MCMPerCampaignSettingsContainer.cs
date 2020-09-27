using MCM.Abstractions;
using MCM.Abstractions.Settings.Base.PerCampaign;
using MCM.Abstractions.Settings.Containers.PerCampaign;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCampaign
{
    internal sealed class MCMPerCampaignSettingsContainer : BasePerCampaignSettingsContainer, IMCMPerCampaignSettingsContainer
    {
        public override void OnGameStarted(Game game)
        {
            LoadedSettings.Clear();

            var settings = new List<PerCampaignSettings>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            var mbOptionScreenSettings = allTypes
                .Where(t => typeof(PerCampaignSettings).IsAssignableFrom(t))
                .Where(t => !typeof(EmptyPerCampaignSettings).IsAssignableFrom(t))
                .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                .Select(t => (PerCampaignSettings) Activator.CreateInstance(t));
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
                RegisterSettings(setting);
        }

        public override void OnGameEnded(Game game)
        {
            LoadedSettings.Clear();
        }
    }
}