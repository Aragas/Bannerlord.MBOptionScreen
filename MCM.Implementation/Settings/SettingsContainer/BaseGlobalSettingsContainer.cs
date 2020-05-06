using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Implementation.Settings.Formats;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace MCM.Implementation.Settings.SettingsContainer
{
    internal abstract class BaseGlobalSettingsContainer : ISettingsContainer
    {
        protected readonly string _defaultRootFolder = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");
        protected Dictionary<string, ISettingsFormat> AvailableSettingsFormats { get; } = new Dictionary<string, ISettingsFormat>();
        protected Dictionary<string, GlobalSettings> LoadedSettings { get; } = new Dictionary<string, GlobalSettings>();

        public List<SettingsDefinition> CreateModSettingsDefinitions => LoadedSettings.Keys
            .Select(id => new SettingsDefinition(id))
            .OrderByDescending(a => a.DisplayName)
            .ToList();

        protected BaseGlobalSettingsContainer()
        {
            foreach (var format in DI.GetImplementations<ISettingsFormat, SettingFormatWrapper>(ApplicationVersionUtils.GameVersion()))
            foreach (var extension in format.Extensions)
            {
                AvailableSettingsFormats[extension] = format;
            }

            if (AvailableSettingsFormats.Count == 0)
                AvailableSettingsFormats.Add("json", new JsonSettingsFormat());
        }

        protected void RegisterSettings(GlobalSettings settings)
        {
            if (settings == null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);

            var path = Path.Combine(_defaultRootFolder, settings.ModuleFolderName, settings.SubFolder ?? "", $"{settings.Id}.{settings.Format}");
            if (AvailableSettingsFormats.ContainsKey(settings.Format))
                AvailableSettingsFormats[settings.Format].Load(settings, path);
            else
                AvailableSettingsFormats["json"].Load(settings, path);
        }

        public BaseSettings? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;
        public bool SaveSettings(BaseSettings settings)
        {
            if (settings == null || !(settings is GlobalSettings globalSettings) || !LoadedSettings.ContainsKey(globalSettings.Id))
                return false;

            var path = Path.Combine(_defaultRootFolder, globalSettings.ModuleFolderName, globalSettings.SubFolder ?? "", $"{globalSettings.Id}.{globalSettings.Format}");
            if (AvailableSettingsFormats.ContainsKey(globalSettings.Format))
                AvailableSettingsFormats[globalSettings.Format].Save(globalSettings, path);
            else
                AvailableSettingsFormats["json"].Save(globalSettings, path);

            return true;
        }

        public bool OverrideSettings(BaseSettings settings)
        {
            if (settings == null || !(settings is GlobalSettings globalSettings) || !LoadedSettings.ContainsKey(globalSettings.Id))
                return false;

            SettingsUtils.OverrideSettings(LoadedSettings[globalSettings.Id], globalSettings, this);
            return true;
        }
        public bool ResetSettings(BaseSettings settings)
        {
            if (settings == null || !(settings is GlobalSettings globalSettings) || !LoadedSettings.ContainsKey(globalSettings.Id))
                return false;

            SettingsUtils.ResetSettings(LoadedSettings[globalSettings.Id], this);
            return true;
        }
    }
}