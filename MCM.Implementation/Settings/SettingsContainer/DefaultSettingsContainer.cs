using MCM.Abstractions.Attributes;
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
    internal sealed class DefaultSettingsContainer : IMBOptionScreenSettingsContainer
    {
        private readonly string _defaultRootFolder = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");
        private Dictionary<string, ISettingsFormat> AvailableSettingsFormats { get; } = new Dictionary<string, ISettingsFormat>();
        private Dictionary<string, SettingsBase> LoadedSettings { get; } = new Dictionary<string, SettingsBase>();

        public List<SettingsDefinition> CreateModSettingsDefinitions => LoadedSettings.Keys
            .Select(id => new SettingsDefinition(id))
            .OrderByDescending(a => a.ModName)
            .ToList();

        public DefaultSettingsContainer()
        {
            foreach (var format in DI.GetImplementations<ISettingsFormat, SettingFormatWrapper>(ApplicationVersionUtils.GameVersion()))
            foreach (var extension in format.Extensions)
            {
                AvailableSettingsFormats[extension] = format;
            }

            if (AvailableSettingsFormats.Count == 0)
                AvailableSettingsFormats.Add("json", new JsonSettingsFormat());
        }

        public bool RegisterSettings(SettingsBase settingsInstance)
        {
            if (settingsInstance == null || LoadedSettings.ContainsKey(settingsInstance.Id) || settingsInstance is ModLibSettings)
                return false;

            LoadedSettings.Add(settingsInstance.Id, settingsInstance);

            var path = Path.Combine(_defaultRootFolder, settingsInstance.ModuleFolderName, settingsInstance.SubFolder ?? "", $"{settingsInstance.Id}.{settingsInstance.Format}");
            if (AvailableSettingsFormats.ContainsKey(settingsInstance.Format))
                AvailableSettingsFormats[settingsInstance.Format].Load(settingsInstance, path);
            else
                AvailableSettingsFormats["json"].Load(settingsInstance, path);

            return true;
        }

        public SettingsBase? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result) ? result : null;
        public void SaveSettings(SettingsBase settingsInstance)
        {
            if (settingsInstance == null || !LoadedSettings.ContainsKey(settingsInstance.Id))
                return;

            var path = Path.Combine(_defaultRootFolder, settingsInstance.ModuleFolderName, settingsInstance.SubFolder ?? "", $"{settingsInstance.Id}.{settingsInstance.Format}");
            if (AvailableSettingsFormats.ContainsKey(settingsInstance.Format))
                AvailableSettingsFormats[settingsInstance.Format].Save(settingsInstance, path);
            else
                AvailableSettingsFormats["json"].Save(settingsInstance, path);
        }

        public bool OverrideSettings(SettingsBase newSettings)
        {
            if (newSettings == null || !LoadedSettings.ContainsKey(newSettings.Id))
                return false;

            SettingsUtils.OverrideSettings(LoadedSettings[newSettings.Id], newSettings, this);
            return true;
        }
        public SettingsBase? ResetSettings(string id) =>
            !LoadedSettings.ContainsKey(id) ? null : SettingsUtils.ResetSettings(LoadedSettings[id], this);
    }
}