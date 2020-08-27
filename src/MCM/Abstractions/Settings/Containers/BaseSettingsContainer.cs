using Bannerlord.ButterLib;
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
        protected Dictionary<string, ISettingsFormat> AvailableSettingsFormats { get; } = new Dictionary<string, ISettingsFormat>();
        protected virtual Dictionary<string, TSettings> LoadedSettings { get; } = new Dictionary<string, TSettings>();

        /// <inheritdoc/>
        public virtual List<SettingsDefinition> CreateModSettingsDefinitions => LoadedSettings.Keys.ToList()
            .Select(id => new SettingsDefinition(id))
            .ToList();

        protected BaseSettingsContainer()
        {
            foreach (var format in ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IEnumerable<ISettingsFormat>>())
            foreach (var extension in format.Extensions)
            {
                AvailableSettingsFormats[extension] = format;
            }
        }

        protected virtual void RegisterSettings(TSettings tSettings)
        {
            if (tSettings == null || LoadedSettings.ContainsKey(tSettings.Id))
                return;

            LoadedSettings.Add(tSettings.Id, tSettings);

            var path = Path.Combine(RootFolder, tSettings.FolderName, tSettings.SubFolder ?? string.Empty, $"{tSettings.Id}.{tSettings.Format}");
            if (AvailableSettingsFormats.ContainsKey(tSettings.Format))
                AvailableSettingsFormats[tSettings.Format].Load(tSettings, path);
            else
                AvailableSettingsFormats["memory"].Load(tSettings, path);
        }

        /// <inheritdoc/>
        public virtual BaseSettings? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;
        /// <inheritdoc/>
        public virtual bool SaveSettings(BaseSettings settings)
        {
            if (!(settings is TSettings tSettings) || !LoadedSettings.ContainsKey(tSettings.Id))
                return false;

            var path = Path.Combine(RootFolder, tSettings.FolderName, tSettings.SubFolder ?? string.Empty, $"{tSettings.Id}.{tSettings.Format}");
            if (AvailableSettingsFormats.ContainsKey(tSettings.Format))
                AvailableSettingsFormats[tSettings.Format].Save(tSettings, path);
            else
                AvailableSettingsFormats["memory"].Save(tSettings, path);

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