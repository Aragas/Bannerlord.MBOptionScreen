using MBOptionScreen.Attributes;

using ModLib;
using ModLib.GUI.ViewModels;
using ModLib.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.SettingDatabase
{
    [SettingsStorageVersion("e1.0.0",  1)]
    [SettingsStorageVersion("e1.0.1",  1)]
    [SettingsStorageVersion("e1.0.2",  1)]
    [SettingsStorageVersion("e1.0.3",  1)]
    [SettingsStorageVersion("e1.0.4",  1)]
    [SettingsStorageVersion("e1.0.5",  1)]
    [SettingsStorageVersion("e1.0.6",  1)]
    [SettingsStorageVersion("e1.0.7",  1)]
    [SettingsStorageVersion("e1.0.8",  1)]
    [SettingsStorageVersion("e1.0.9",  1)]
    [SettingsStorageVersion("e1.0.10", 1)]
    [SettingsStorageVersion("e1.0.11", 1)]
    [SettingsStorageVersion("e1.1.0",  1)]
    internal class DefaultSettingsStorage : ISettingsStorage
    {
        private List<ModSettingsVM> _modSettings = null;

        private Dictionary<string, SettingsBase> AllSettingsDict { get; } = new Dictionary<string, SettingsBase>();

        public List<SettingsBase> AllSettings => AllSettingsDict.Values.ToList();
        public int SettingsCount => AllSettingsDict.Values.Count;
        public List<ModSettingsVM> ModSettingsVMs => _modSettings ??= GetModSettingsVMs().ToList();

        public DefaultSettingsStorage()
        {
            var settings = new MBOptionScreenSettings();
            AllSettingsDict.Add(settings.ID, settings);
#if DEBUG
            var testSettings = new TestSettings();
            AllSettingsDict.Add(testSettings.ID, testSettings);
            
#endif
        }

        public bool RegisterSettings(SettingsBase settingsClass)
        {
            if (!AllSettingsDict.ContainsKey(settingsClass.ID))
            {
                AllSettingsDict.Add(settingsClass.ID, settingsClass);
                return true;
            }
            else
            {
                //TODO:: When debugging log is finished, show a message saying that the key already exists
                return false;
            }
        }

        public ISerialisableFile? GetSettings(string uniqueId) => AllSettingsDict.TryGetValue(uniqueId, out var val) ? val : null;

        public void SaveSettings(SettingsBase settingsInstance)
        {
            FileDatabase.SaveToFile(settingsInstance.ModuleFolderName, settingsInstance, Location.Configs);
        }

        public IEnumerable<ModSettingsVM> GetModSettingsVMs()
        {
            try
            {
                return AllSettings
                    .Select(settings => new ModSettingsVM(settings))
                    .OrderByDescending(a => a.ModName != MBOptionScreenSettings.Instance!.ID)
                    .ThenBy(a => a.ModName);
            }
            catch (Exception ex)
            {
                return new List<ModSettingsVM>();
                // TODO
                //ModDebug.ShowError("An error occurred while creating the ViewModels for all mod settings", "Error Occurred", ex);
            }
        }
    }
}