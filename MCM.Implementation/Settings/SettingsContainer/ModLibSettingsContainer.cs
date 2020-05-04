using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    internal sealed class ModLibSettingsContainer : IModLibSettingsContainer
    {
        private Dictionary<string, ModLibSettings> LoadedModLibSettings { get; } = new Dictionary<string, ModLibSettings>();
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
                    .OrderByDescending(a => a.ModName)
                    .ToList();
            }
        }

        public ModLibSettingsContainer()
        {
            ModLibSettingsDatabase = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(a => a.FullName == "ModLib.SettingsDatabase");
        }

        public bool RegisterSettings(SettingsBase settingsClass) => settingsClass is ModLibSettings;

        public SettingsBase? GetSettings(string id)
        {
            if (ModLibSettingsDatabase == null)
                return null;

            Reload(id);
            return LoadedModLibSettings.TryGetValue(id, out var settings) ? settings : null;
        }
        public void SaveSettings(SettingsBase settings)
        {
            if (ModLibSettingsDatabase == null)
                return;

            if (settings is ModLibSettings settingsWrapper)
                AccessTools.Method(ModLibSettingsDatabase, "SaveSettings")?.Invoke(null, new object[] { settingsWrapper.Object });
        }

        public bool OverrideSettings(SettingsBase newSettings)
        {
            if (ModLibSettingsDatabase == null)
                return false;

            SettingsUtils.OverrideSettings(LoadedModLibSettings[newSettings.Id], newSettings, this);
            return true;
        }
        public SettingsBase? ResetSettings(string id) =>
            ModLibSettingsDatabase == null ? null : SettingsUtils.ResetSettings(LoadedModLibSettings[id], this);


        private void ReloadAll()
        {
            var saveSettingsMethod = AccessTools.Property(ModLibSettingsDatabase, "AllSettingsDict");
            var dict = (IDictionary) saveSettingsMethod.GetValue(null);
            foreach (var settings in dict.Values)
            {
                var id = AccessTools.Property(settings.GetType(), "ID")?.GetValue(settings) as string ?? "ERROR";
                if (!LoadedModLibSettings.ContainsKey(id))
                    LoadedModLibSettings.Add(id, new ModLibSettings(settings));
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