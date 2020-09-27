using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.PerCampaign;
using MCM.Abstractions.Settings.Formats;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCampaign
{
    public abstract class BasePerCampaignSettingsContainer : BaseSettingsContainer<PerCampaignSettings>, IPerCampaignSettingsContainer
    {
        /// <inheritdoc/>
        protected override string RootFolder { get; }

        protected BasePerCampaignSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "PerCampaign");
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(PerCampaignSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return;

            if (settings == null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return false;

            if (!(settings is PerCampaignSettings) || !LoadedSettings.ContainsKey(settings.Id))
                return false;

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);

            return true;
        }

        /// <inheritdoc/>
        public override bool ResetSettings(BaseSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return false;

            return base.ResetSettings(settings);
        }
        /// <inheritdoc/>
        public override bool OverrideSettings(BaseSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return false;

            return base.OverrideSettings(settings);
        }

        /// <inheritdoc/>
        public abstract void OnGameStarted(Game game);
        /// <inheritdoc/>
        public abstract void OnGameEnded(Game game);
    }
}