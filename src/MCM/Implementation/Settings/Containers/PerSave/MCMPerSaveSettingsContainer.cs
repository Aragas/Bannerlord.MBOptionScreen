using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

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
        private readonly IBUTRLogger _logger;

        public MCMPerSaveSettingsContainer(IBUTRLogger<MCMPerSaveSettingsContainer> logger)
        {
            _logger = logger;
        }

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
            IEnumerable<PerSaveSettings> GetPerSaveSettings()
            {
                foreach (var assembly in AccessTools2.AllAssemblies().Where(a => !a.IsDynamic))
                {
                    IEnumerable<PerSaveSettings> settings;
                    try
                    {
                        settings = AccessTools2.GetTypesFromAssemblyIfValid(assembly)
                            .Where(t => t.IsClass && !t.IsAbstract)
                            .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                            .Where(t => typeof(PerSaveSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(EmptyPerSaveSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                            .Select(t => Activator.CreateInstance(t) as PerSaveSettings)
                            .OfType<PerSaveSettings>()
                            .ToList();
                    }
                    catch (TypeLoadException ex)
                    {
                        settings = Array.Empty<PerSaveSettings>();
                        _logger.LogError(ex, "Error while handling assembly {Assembly}!", assembly);
                    }

                    foreach (var setting in settings)
                    {
                        yield return setting;
                    }
                }
            }

            foreach (var setting in GetPerSaveSettings())
                RegisterSettings(setting);
        }

        public void OnGameEnded(Game game)
        {
            LoadedSettings.Clear();
        }
    }
}