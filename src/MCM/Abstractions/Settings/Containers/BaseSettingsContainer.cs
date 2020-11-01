using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Models;
using MCM.Utils;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace MCM.Abstractions.Settings.Containers
{
    public abstract class BaseSettingsContainer<TSettings> : ISettingsContainer where TSettings : BaseSettings
    {
        protected virtual string RootFolder { get; } = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");
        protected virtual Dictionary<string, TSettings> LoadedSettings { get; } = new Dictionary<string, TSettings>();

        /// <inheritdoc/>
        public virtual List<SettingsDefinition> CreateModSettingsDefinitions => LoadedSettings.Keys.ToList()
            .ConvertAll(id => new SettingsDefinition(id));

        protected virtual void RegisterSettings(TSettings? settings)
        {
            if (settings is null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);
        }

        /// <inheritdoc/>
        public virtual BaseSettings? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;
        /// <inheritdoc/>
        public virtual bool SaveSettings(BaseSettings settings)
        {
            if (!(settings is TSettings tSettings) || !LoadedSettings.ContainsKey(tSettings.Id))
                return false;

            var directoryPath = Path.Combine(RootFolder, tSettings.FolderName, tSettings.SubFolder);
            var settingsFormats = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Save(settings, directoryPath, settings.Id);

            return true;
        }

        /// <inheritdoc/>
        public virtual bool OverrideSettings(BaseSettings settings)
        {
            if (!(settings is TSettings tSettings) || !LoadedSettings.ContainsKey(tSettings.Id))
                return false;

            SettingsUtils.OverrideSettings(LoadedSettings[tSettings.Id], tSettings);
            return true;
        }
        /// <inheritdoc/>
        public virtual bool ResetSettings(BaseSettings settings)
        {
            if (!(settings is TSettings tSettings) || !LoadedSettings.ContainsKey(tSettings.Id))
                return false;

            SettingsUtils.ResetSettings(LoadedSettings[tSettings.Id]);
            return true;
        }
    }
}