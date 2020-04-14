using MBOptionScreen;
using MBOptionScreen.SettingDatabase;
using ModLib.GUI.ViewModels;
using ModLib.Interfaces;
using System.Collections.Generic;

namespace ModLib
{
    internal static class SettingsDatabase
    {
        private static ISettingsStorage SettingsStorage => MBOptionScreenSubModule.SyncObject.SettingsStorage;

        public static List<SettingsBase> AllSettings => SettingsStorage.AllSettings;
        public static int SettingsCount => SettingsStorage.SettingsCount;
        public static List<ModSettingsVM> ModSettingsVMs => SettingsStorage.ModSettingsVMs;

        public static bool RegisterSettings(SettingsBase settingsClass) =>
            SettingsStorage.RegisterSettings(settingsClass);

        public static ISerialisableFile? GetSettings(string uniqueId) =>
            SettingsStorage.GetSettings(uniqueId);

        public static void SaveSettings(SettingsBase settingsInstance) =>
            SettingsStorage.SaveSettings(settingsInstance);

        public static bool OverrideSettingsWithID(SettingsBase settings, string ID)
        {
            /*
            if (AllSettingsDict.ContainsKey(ID))
            {
                AllSettingsDict[ID] = settings;
                return true;
            }
            return false;
            */
            return false;
        }

        /// <summary>
        /// Resets the settings instance to the default values for that instance.
        /// </summary>
        /// <param name="settingsInstance">The instance of the object to be reset</param>
        /// <returns>Returns the instance of the new object with default values.</returns>
        public static SettingsBase ResetSettingsInstance(SettingsBase settingsInstance)
        {
            /*
            string id = settingsInstance.ID;
            SettingsBase newObj = (SettingsBase)Activator.CreateInstance(settingsInstance.GetType());
            newObj.ID = id;
            AllSettingsDict[id] = newObj;
            return newObj;
            */
            return null;
        }
    }
}