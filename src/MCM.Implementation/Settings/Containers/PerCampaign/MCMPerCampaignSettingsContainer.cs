using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.PerCampaign;
using MCM.Abstractions.Settings.Containers.PerCampaign;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCampaign
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    [Version("e1.4.1",  1)]
    internal sealed class MCMPerCampaignSettingsContainer : BasePerCampaignSettingsContainer, IMCMPerCampaignSettingsContainer
    {
        public override void OnGameStarted(Game game)
        {
            LoadedSettings.Clear();

            var settings = new List<PerCampaignSettings>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                // ignore v1 and v2 classes
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            var mbOptionScreenSettings = allTypes
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(PerCampaignSettings)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(EmptyPerCampaignSettings)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(IWrapper)))
                .Select(obj => BasePerCampaignSettingsWrapper.Create(Activator.CreateInstance(obj)));
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