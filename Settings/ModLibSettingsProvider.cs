using HarmonyLib;

using MBOptionScreen.Interfaces;
using MBOptionScreen.Settings.Wrapper;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
{
    internal class ModLibSettingsProviderWrapper : ISettingsProvider
    {
        private Dictionary<string, SettingsWrapper> LoadedModLibSettings { get; } = new Dictionary<string, SettingsWrapper>();
        private Type ModLibSettingsDatabase { get; }

        public List<ModSettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                ReloadAll();

                return LoadedModLibSettings.Keys
                    .Select(id => new ModSettingsDefinition(id))
                    .OrderByDescending(a => a.ModName)
                    .ToList();
            }
        }

        public ModLibSettingsProviderWrapper()
        {
            ModLibSettingsDatabase = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .FirstOrDefault(a => a.FullName == "ModLib.SettingsDatabase");
        }

        public SettingsBase? GetSettings(string id)
        {
            ReloadAll();
            return LoadedModLibSettings.TryGetValue(id, out var setings) ? setings : null;
        }

        public bool OverrideSettings(SettingsBase settings)
        {
            if (settings is SettingsWrapper settingsWrapper)
            {
                var overrideSettingsWithIDMethod = AccessTools.Method(ModLibSettingsDatabase, "OverrideSettingsWithID");

                return (bool) overrideSettingsWithIDMethod.Invoke(null, new object[] { settingsWrapper._object, settingsWrapper.Id });
            }

            return false;
        }

        public bool RegisterSettings(SettingsBase settingsClass) => true;

        public SettingsBase ResetSettings(string id)
        {
            ReloadAll();

            var resetSettingsInstanceMethod = AccessTools.Method(ModLibSettingsDatabase, "ResetSettingsInstance");

            var settingsWrapper = LoadedModLibSettings[id];
            resetSettingsInstanceMethod.Invoke(null, new object[] { settingsWrapper._object });

            Reload(id);
            return LoadedModLibSettings[id];
        }

        public void SaveSettings(SettingsBase settings)
        {
            if (settings is SettingsWrapper settingsWrapper)
            {
                var saveSettingsMethod = AccessTools.Method(ModLibSettingsDatabase, "SaveSettings");

                saveSettingsMethod.Invoke(null, new object[] { settingsWrapper._object });
            }
        }

        private void ReloadAll()
        {
            var saveSettingsMethod = AccessTools.Property(ModLibSettingsDatabase, "AllSettingsDict");
            var dict = (IDictionary)saveSettingsMethod.GetValue(null);
            foreach (var settings in dict.Values)
            {
                var settingsType = settings.GetType();
                var idProperty = AccessTools.Property(settingsType, "ID") ?? AccessTools.Property(settingsType, "Id");
                var id = (string) idProperty.GetValue(settings);

                var settWrapper = new ModLibSettingsWrapper(settings);

                if (!LoadedModLibSettings.ContainsKey(id))
                    LoadedModLibSettings.Add(id, settWrapper);
                else
                    LoadedModLibSettings[id] = settWrapper;
            }
        }
        private void Reload(string id)
        {
            var saveSettingsMethod = AccessTools.Property(ModLibSettingsDatabase, "AllSettingsDict");
            var dict = (IDictionary) saveSettingsMethod.GetValue(null);

            if (dict.Contains(id))
                LoadedModLibSettings[id] = new AttributeSettingsWrapper(dict[id]);
        }
    }
}