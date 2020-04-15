using MBOptionScreen.Attributes;
using MBOptionScreen.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
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
        private Dictionary<string, SettingsBase> AllSettingsDict { get; } = new Dictionary<string, SettingsBase>();

        public List<SettingsBase> AllSettings => AllSettingsDict.Values.ToList();
        public int SettingsCount => AllSettingsDict.Values.Count;
        public List<ModSettingsDefinition> ModSettingsVMs => GetModSettingsVMs().ToList();

        public DefaultSettingsStorage()
        {
            var settings = new MBOptionScreenSettings();
            AllSettingsDict.Add(settings.Id, settings);
#if DEBUG
            var testSettings = new TestSettings();
            AllSettingsDict.Add(testSettings.Id, testSettings);
            
#endif
        }

        public bool RegisterSettings(SettingsBase settingsClass)
        {
            if (!AllSettingsDict.ContainsKey(settingsClass.Id))
            {
                AllSettingsDict.Add(settingsClass.Id, settingsClass);
                return true;
            }
            else
            {
                //TODO:: When debugging log is finished, show a message saying that the key already exists
                return false;
            }
        }

        public ISerializableFile? GetSettings(string uniqueId) => AllSettingsDict.TryGetValue(uniqueId, out var val) ? val : null;

        public void SaveSettings(SettingsBase settingsInstance)
        {
            FileDatabase.SaveToFile(settingsInstance.ModuleFolderName, settingsInstance, Location.Configs);
        }

        public IEnumerable<ModSettingsDefinition> GetModSettingsVMs()
        {
            try
            {
                return AllSettings
                    .Select(settings => new ModSettingsDefinition(settings))
                    .OrderByDescending(a => a.ModName != MBOptionScreenSettings.Instance!.Id)
                    .ThenBy(a => a.ModName);
            }
            catch (Exception ex)
            {
                return new List<ModSettingsDefinition>();
                // TODO
                //ModDebug.ShowError("An error occurred while creating the ViewModels for all mod settings", "Error Occurred", ex);
            }
        }

        public bool OverrideSettingsWithId(SettingsBase settings, string Id)
        {
            if (AllSettingsDict.ContainsKey(Id))
            {
                AllSettingsDict[Id] = settings;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the settings instance to the default values for that instance.
        /// </summary>
        /// <param name="settingsInstance">The instance of the object to be reset</param>
        /// <returns>Returns the instance of the new object with default values.</returns>
        public SettingsBase ResetSettingsInstance(SettingsBase settingsInstance)
        {
            var id = settingsInstance.Id;
            var newObj = (SettingsBase) Activator.CreateInstance(settingsInstance.GetType());
            newObj.Id = id;
            AllSettingsDict[id] = newObj;
            return newObj;
        }
    }
}