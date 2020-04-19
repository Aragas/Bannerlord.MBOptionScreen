using System;
using System.Collections.Generic;
using System.Linq;

namespace ModLib
{
    internal static class SettingsDatabase
    {
        private static Dictionary<string, object> AllSettingsDict { get; } = new Dictionary<string, object>();

        public static List<object> AllSettings => AllSettingsDict.Values.ToList();
        public static int SettingsCount => AllSettingsDict.Values.Count;

        /// <summary>
        /// Registers the settings class with the SettingsDatabase for use in the settings menu.
        /// </summary>
        /// <param name="settings">Intance of the settings object to be registered with the SettingsDatabase.</param>
        /// <returns>Returns true if successful. Returns false if the object's ID key is already present in the SettingsDatabase.</returns>
        public static bool RegisterSettings(object settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (!AllSettingsDict.ContainsKey(Utils.GetID(settings)))
            {
                AllSettingsDict.Add(Utils.GetID(settings), settings);
                return true;
            }
            else
            {
                //TODO:: When debugging log is finished, show a message saying that the key already exists
                return false;
            }
        }

        /// <summary>
        /// Retrieves the Settings instance from the SettingsDatabase with the given ID.
        /// </summary>
        /// <param name="uniqueID">The ID for the settings instance.</param>
        /// <returns>Returns the settings instance with the given ID. Returns null if nothing can be found.</returns>
        public static object GetSettings(string uniqueID)
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
        public static bool SaveSettings(object settingsInstance)
        {
            if (settingsInstance == null) throw new ArgumentNullException(nameof(settingsInstance));
            return FileDatabase.SaveToFile(Utils.GetModuleFolderName(settingsInstance), settingsInstance, FileDatabase.Location.Configs);
        }

        /// <summary>
        /// Resets the settings instance to the default values for that instance.
        /// </summary>
        /// <param name="settingsInstance">The instance of the object to be reset</param>
        /// <returns>Returns the instance of the new object with default values.</returns>
        public static object ResetSettingsInstance(object settingsInstance)
        {
            if (settingsInstance == null) throw new ArgumentNullException(nameof(settingsInstance));
            string id = Utils.GetID(settingsInstance);
            var newObj = Activator.CreateInstance(settingsInstance.GetType());
            Utils.SetID(newObj, id);
            AllSettingsDict[id] = newObj;
            return newObj;
        }

        internal static bool OverrideSettingsWithID(object settings, string ID)
        {
            if (AllSettingsDict.ContainsKey(ID))
            {
                AllSettingsDict[ID] = settings;
                return true;
            }
            return false;
        }
    }
}
