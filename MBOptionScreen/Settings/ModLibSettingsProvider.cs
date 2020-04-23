using HarmonyLib;

using MBOptionScreen.Attributes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    internal sealed class ModLibSettingsProvider : IModLibSettingsProvider
    {
        private Dictionary<string, SettingsWrapper> LoadedModLibSettings { get; } = new Dictionary<string, SettingsWrapper>();
        private Type ModLibSettingsDatabase { get; }

        public List<ModSettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                if (ModLibSettingsDatabase == null)
                    return new List<ModSettingsDefinition>();

                ReloadAll();

                return LoadedModLibSettings.Keys
                    .Select(id => new ModSettingsDefinition(id))
                    .OrderByDescending(a => a.ModName)
                    .ToList();
            }
        }

        public ModLibSettingsProvider()
        {
            ModLibSettingsDatabase = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(a => a.FullName == "ModLib.SettingsDatabase");
        }

        public SettingsBase? GetSettings(string id)
        {
            if (ModLibSettingsDatabase == null)
                return null;

            ReloadAll();
            return LoadedModLibSettings.TryGetValue(id, out var setings) ? setings : null;
        }

        public bool OverrideSettings(SettingsBase settings)
        {
            if (ModLibSettingsDatabase == null)
                return false;

            if (settings is SettingsWrapper settingsWrapper)
            {
                var overrideSettingsWithIDMethod = AccessTools.Method(ModLibSettingsDatabase, "OverrideSettingsWithID");

                return (bool) overrideSettingsWithIDMethod.Invoke(null, new object[] { settingsWrapper._object, settingsWrapper.Id });
            }

            return false;
        }

        public bool RegisterSettings(SettingsBase settingsClass)
        {
            //if (BuiltInModLib)
            //{
            //    if (settingsClass is SettingsWrapper settingsWrapper)
            //    {
            //        var registerSettingsMethod = AccessTools.Method(ModLibSettingsDatabase, "RegisterSettings");
            //        registerSettingsMethod.Invoke(null, new object[] { settingsWrapper._object });
            //        return true;
            //    }
            //}

            return ModLibSettingsDatabase != null;
        }

        public SettingsBase ResetSettings(string id)
        {
            if (ModLibSettingsDatabase == null)
                return null;

            ReloadAll();

            var resetSettingsInstanceMethod = AccessTools.Method(ModLibSettingsDatabase, "ResetSettingsInstance");

            var settingsWrapper = LoadedModLibSettings[id];
            resetSettingsInstanceMethod.Invoke(null, new object[] { settingsWrapper._object });

            Reload(id);
            return LoadedModLibSettings[id];
        }

        public void SaveSettings(SettingsBase settings)
        {
            if (ModLibSettingsDatabase == null)
                return;

            if (settings is SettingsWrapper settingsWrapper)
            {
                var saveSettingsMethod = AccessTools.Method(ModLibSettingsDatabase, "SaveSettings");

                saveSettingsMethod.Invoke(null, new object[] { settingsWrapper._object });
            }
        }

        private void ReloadAll()
        {
            var saveSettingsMethod = AccessTools.Property(ModLibSettingsDatabase, "AllSettingsDict");
            var dict = (IDictionary) saveSettingsMethod.GetValue(null);
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
                LoadedModLibSettings[id] = new SettingsWrapper(dict[id]);
        }
    }
}