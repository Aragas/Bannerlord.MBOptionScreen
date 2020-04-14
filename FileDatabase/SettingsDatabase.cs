using ModLib.Debugging;
using ModLib.GUI.ViewModels;
using ModLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModLib
{
    public static class SettingsDatabase
    {
        private static List<ModSettingsVM> _modSettingsVMs = null;
        private static Dictionary<string, SettingsBase> AllSettingsDict { get; } = new Dictionary<string, SettingsBase>();

        public static List<SettingsBase> AllSettings => AllSettingsDict.Values.ToList();
        public static int SettingsCount => AllSettingsDict.Values.Count;
        public static List<ModSettingsVM> ModSettingsVMs
        {
            get
            {
                if (_modSettingsVMs == null)
                {
                    BuildModSettingsVMs();
                }
                return _modSettingsVMs;
            }
        }

        /// <summary>
        /// Registers the settings class with the SettingsDatabase for use in the settings menu.
        /// </summary>
        /// <param name="settings">Intance of the settings object to be registered with the SettingsDatabase.</param>
        /// <returns>Returns true if successful. Returns false if the object's ID key is already present in the SettingsDatabase.</returns>
        public static bool RegisterSettings(SettingsBase settings)
        {
            if (!AllSettingsDict.ContainsKey(settings.ID))
            {
                AllSettingsDict.Add(settings.ID, settings);
                _modSettingsVMs = null;
                return true;
            }
            else
            {
                //TODO:: When debugging log is finished, show a message saying that the key already exists
                return false;
            }
        }

        internal static bool OverrideSettingsWithID(SettingsBase settings, string ID)
        {
            if (AllSettingsDict.ContainsKey(ID))
            {
                AllSettingsDict[ID] = settings;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves the Settings instance from the SettingsDatabase with the given ID.
        /// </summary>
        /// <param name="uniqueID">The ID for the settings instance.</param>
        /// <returns>Returns the settings instance with the given ID. Returns null if nothing can be found.</returns>
        public static ISerialisableFile GetSettings(string uniqueID)
        {
            if (AllSettingsDict.ContainsKey(uniqueID))
            {
                return AllSettingsDict[uniqueID];
            }
            else
                return null;
        }

        /// <summary>
        /// Saves the settings instance to file.
        /// </summary>
        /// <param name="settingsInstance">Instance of the settings object to save to file.</param>
        /// <returns>Return true if the settings object was saved successfully. Returns false if it failed to save.</returns>
        public static bool SaveSettings(SettingsBase settingsInstance)
        {
            return FileDatabase.SaveToFile(settingsInstance.ModuleFolderName, settingsInstance, FileDatabase.Location.Configs);
        }

        /// <summary>
        /// Resets the settings instance to the default values for that instance.
        /// </summary>
        /// <param name="settingsInstance">The instance of the object to be reset</param>
        /// <returns>Returns the instance of the new object with default values.</returns>
        public static SettingsBase ResetSettingsInstance(SettingsBase settingsInstance)
        {
            string id = settingsInstance.ID;
            SettingsBase newObj = (SettingsBase)Activator.CreateInstance(settingsInstance.GetType());
            newObj.ID = id;
            AllSettingsDict[id] = newObj;
            return newObj;
        }

        internal static void BuildModSettingsVMs()
        {
            try
            {
                _modSettingsVMs = new List<ModSettingsVM>();
                foreach (var settings in AllSettings)
                {
                    ModSettingsVM msvm = new ModSettingsVM(settings);
                    _modSettingsVMs.Add(msvm);
                }
                _modSettingsVMs.Sort((x, y) => y.ModName.CompareTo(x.ModName));
            }
            catch (Exception ex)
            {
                ModDebug.ShowError("An error occurred while creating the ViewModels for all mod settings", "Error Occurred", ex);
            }
        }
    }
}
