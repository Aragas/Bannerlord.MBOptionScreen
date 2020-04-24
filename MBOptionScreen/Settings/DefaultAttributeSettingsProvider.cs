using MBOptionScreen.Attributes;
using MBOptionScreen.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace MBOptionScreen.Settings
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    internal sealed class DefaultAttributeSettingsProvider : IMBOptionScreenSettingsProvider
    {
        private readonly string _defaultRootFolder = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");
        private Dictionary<string, ISettingsFormat> AvailableSettingsFormats { get; } = new Dictionary<string, ISettingsFormat>();
        private Dictionary<string, SettingsBase> LoadedSettings { get; } = new Dictionary<string, SettingsBase>();

        public List<ModSettingsDefinition> CreateModSettingsDefinitions => LoadedSettings.Keys
            .Select(id => new ModSettingsDefinition(id))
            .OrderByDescending(a => a.ModName)
            .ToList();

        public DefaultAttributeSettingsProvider()
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
            if (settingsInstance == null || LoadedSettings.ContainsKey(settingsInstance.Id))
                return false;

            LoadedSettings.Add(settingsInstance.Id, settingsInstance);

            var path = Path.Combine(_defaultRootFolder, settingsInstance.ModuleFolderName, settingsInstance.SubFolder ?? "", $"{settingsInstance.Id}.{settingsInstance.Format}");
            if (AvailableSettingsFormats.ContainsKey(settingsInstance.Format))
                AvailableSettingsFormats[settingsInstance.Format].Load(settingsInstance, path);
            else
                AvailableSettingsFormats["json"].Load(settingsInstance, path);

            return true;
        }

        public SettingsBase? GetSettings(string id) => LoadedSettings.TryGetValue(id, out var result)
            ? result
            : null;

        public void SaveSettings(SettingsBase settingsInstance)
        {
            if (settingsInstance == null || !LoadedSettings.ContainsKey(settingsInstance.Id))
                return;

            var path = Path.Combine(_defaultRootFolder, settingsInstance.ModuleFolderName, settingsInstance.SubFolder ?? "", $"{settingsInstance.Id}.json");
            if (AvailableSettingsFormats.ContainsKey(settingsInstance.Format))
                AvailableSettingsFormats[settingsInstance.Format].Save(settingsInstance, path);
            else
                AvailableSettingsFormats["json"].Save(settingsInstance, path);
        }

        public bool OverrideSettings(SettingsBase newSettingsInstance)
        {
            if (newSettingsInstance == null || !LoadedSettings.ContainsKey(newSettingsInstance.Id))
                return false;

            LoadedSettings[newSettingsInstance.Id] = newSettingsInstance;
            SaveSettings(newSettingsInstance);
            return true;
        }

        public SettingsBase ResetSettings(string id)
        {
            if (!LoadedSettings.ContainsKey(id))
                return null;

            // TODO:
            var defaultSettingsInstance = LoadedSettings[id] is SettingsWrapper wrapperSettings
                ? new SettingsWrapper(Activator.CreateInstance(wrapperSettings._object.GetType()))
                : (SettingsBase) Activator.CreateInstance(LoadedSettings[id].GetType());
            LoadedSettings[id] = defaultSettingsInstance;
            SaveSettings(defaultSettingsInstance);
            return defaultSettingsInstance;
        }
    }
}