using BUTR.DependencyInjection;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.PerSave;
using MCM.Abstractions.Settings.Containers;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerSave
{
    internal sealed class MCMPerSaveSettingsContainer : BaseSettingsContainer<PerSaveSettings>, IMCMPerSaveSettingsContainer
    {
        /// <inheritdoc/>
        protected override void RegisterSettings(PerSaveSettings? perSaveSettings)
        {
            var behavior = GenericServiceProvider.GetService<PerSaveCampaignBehavior>();
            if (behavior is null)
                return;

            if (perSaveSettings is null)
                return;

            LoadedSettings.Add(perSaveSettings.Id, perSaveSettings);

            behavior.LoadSettings(perSaveSettings);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            var behavior = GenericServiceProvider.GetService<PerSaveCampaignBehavior>();
            if (behavior is null)
                return false;

            if (settings is not PerSaveSettings saveSettings || !LoadedSettings.ContainsKey(saveSettings.Id))
                return false;

            return behavior.SaveSettings(saveSettings);
        }

        public void OnGameStarted(Game game)
        {
            LoadedSettings.Clear();
        }

        public void LoadSettings()
        {
            var settings = new List<PerSaveSettings>();
            var allTypes = AccessTools2.AllAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(AccessTools2.GetTypesFromAssembly)
                .Where(t => t.IsClass && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null)
                .ToList();

            var mbOptionScreenSettings = allTypes
                .Where(t => typeof(PerSaveSettings).IsAssignableFrom(t))
                .Where(t => !typeof(EmptyPerSaveSettings).IsAssignableFrom(t))
                .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as PerSaveSettings)
                .Where(t => t is not null)
                .Cast<PerSaveSettings>();
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
                RegisterSettings(setting);
        }

        public void OnGameEnded(Game game)
        {
            LoadedSettings.Clear();
        }
    }
}