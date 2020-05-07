using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.ModLib.Settings.SettingsContainer
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
    internal sealed class ModLibSettingsContainer : IModLibSettingsContainer
    {
        private Dictionary<string, ModLibGlobalSettingsWrapper> LoadedModLibSettings { get; } = new Dictionary<string, ModLibGlobalSettingsWrapper>();
        private Type? ModLibSettingsDatabase { get; }

        public List<SettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                if (ModLibSettingsDatabase == null)
                    return new List<SettingsDefinition>();

                ReloadAll();

                return LoadedModLibSettings.Keys
                    .Select(id => new SettingsDefinition(id))
                    .OrderByDescending(a => a.DisplayName)
                    .ToList();
            }
        }

        public ModLibSettingsContainer()
        {
            ModLibSettingsDatabase = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(a => a.FullName == "ModLib.SettingsDatabase");
        }

        public BaseSettings? GetSettings(string id)
        {
            if (ModLibSettingsDatabase == null)
                return null;

            Reload(id);
            return LoadedModLibSettings.TryGetValue(id, out var settings) ? settings : null;
        }
        public bool SaveSettings(BaseSettings settings)
        {
            if (ModLibSettingsDatabase == null || !(settings is ModLibGlobalSettingsWrapper settingsWrapper))
                return false;

            AccessTools.Method(ModLibSettingsDatabase, "SaveSettings")?.Invoke(null, new object[] { settingsWrapper.Object });
            return true;
        }

        public bool OverrideSettings(BaseSettings newSettings)
        {
            if (ModLibSettingsDatabase == null)
                return false;

            SettingsUtils.OverrideSettings(LoadedModLibSettings[newSettings.Id], newSettings, this);
            return true;
        }
        public bool ResetSettings(BaseSettings settings)
        {
            if (ModLibSettingsDatabase == null || !(settings is ModLibGlobalSettingsWrapper))
                return false;

            SettingsUtils.ResetSettings(LoadedModLibSettings[settings.Id], this);
            return true;
        }


        private void ReloadAll()
        {
            var saveSettingsMethod = AccessTools.Property(ModLibSettingsDatabase, "AllSettingsDict");
            var dict = (IDictionary) saveSettingsMethod.GetValue(null);
            foreach (var settings in dict.Values)
            {
                var id = AccessTools.Property(settings.GetType(), "ID")?.GetValue(settings) as string ?? "ERROR";
                if (!LoadedModLibSettings.ContainsKey(id))
                    LoadedModLibSettings.Add(id, new ModLibGlobalSettingsWrapper(settings));
                else
                    LoadedModLibSettings[id].UpdateReference(settings);
            }
        }
        private void Reload(string id)
        {
            var saveSettingsMethod = AccessTools.Property(ModLibSettingsDatabase, "AllSettingsDict");
            var dict = (IDictionary) saveSettingsMethod.GetValue(null);

            if (dict.Contains(id))
                LoadedModLibSettings[id].UpdateReference(dict[id]);
        }
    }
}