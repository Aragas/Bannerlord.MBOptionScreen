using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerCampaign;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCM.Implementation.PerCampaign
{
    internal sealed class PerCampaignSettingsContainer : BaseSettingsContainer<PerCampaignSettings>, IPerCampaignSettingsContainer
    {
        private readonly IBUTRLogger _logger;
        private readonly IGameEventListener _gameEventListener;

        /// <inheritdoc/>
        protected override string RootFolder { get; }

        public PerCampaignSettingsContainer(IBUTRLogger<PerCampaignSettingsContainer> logger, IGameEventListener gameEventListener)
        {
            _logger = logger;
            _gameEventListener = gameEventListener;
            _gameEventListener.OnGameStarted += OnGameStarted;
            _gameEventListener.OnGameEnded += OnGameEnded;
            RootFolder = Path.Combine(base.RootFolder, "PerCampaign");
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(PerCampaignSettings? perCampaignSettings)
        {
            if (perCampaignSettings is null)
                return;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
                return;

            var directoryPath = Path.Combine(RootFolder, id, perCampaignSettings.FolderName, perCampaignSettings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == perCampaignSettings.FormatType));
            settingsFormat?.Load(perCampaignSettings, directoryPath, perCampaignSettings.Id);
            perCampaignSettings.OnPropertyChanged(BaseSettings.LoadingComplete);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            if (settings is not PerCampaignSettings campaignSettings)
                return false;

            if (GenericServiceProvider.GetService<ICampaignIdProvider>() is not { } campaignIdProvider || campaignIdProvider.GetCurrentCampaignId() is not { } id)
                return false;

            var directoryPath = Path.Combine(RootFolder, id, campaignSettings.FolderName, campaignSettings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == campaignSettings.FormatType));
            settingsFormat?.Save(campaignSettings, directoryPath, campaignSettings.Id);

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
            return true;
        }

        private void OnGameStarted()
        {
            LoadedSettings.Clear();

            LoadSettings();
        }

        private void LoadSettings()
        {
            IEnumerable<PerCampaignSettings> GetPerCampaignSettings()
            {
                foreach (var assembly in AccessTools2.AllAssemblies().Where(a => !a.IsDynamic))
                {
                    IEnumerable<PerCampaignSettings> settings;
                    try
                    {
                        settings = AccessTools2.GetTypesFromAssemblyIfValid(assembly)
                            .Where(t => t.IsClass && !t.IsAbstract)
                            .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                            .Where(t => typeof(PerCampaignSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(EmptyPerCampaignSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                            .Select(t => Activator.CreateInstance(t) as PerCampaignSettings)
                            .OfType<PerCampaignSettings>()
                            .ToList();
                    }
                    catch (TypeLoadException ex)
                    {
                        settings = Array.Empty<PerCampaignSettings>();
                        _logger.LogError(ex, $"Error while handling assembly {assembly}!");
                    }

                    foreach (var setting in settings)
                    {
                        yield return setting;
                    }
                }
            }

            foreach (var setting in GetPerCampaignSettings())
                RegisterSettings(setting);
        }

        private void OnGameEnded()
        {
            LoadedSettings.Clear();
        }
    }
}