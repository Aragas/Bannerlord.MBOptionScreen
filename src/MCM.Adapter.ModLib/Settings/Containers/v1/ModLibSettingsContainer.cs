extern alias v1;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Adapter.ModLib.Settings.Base.v1;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

using v1SettingsBase = v1::ModLib.SettingsBase;
using v1SettingsDatabase = v1::ModLib.SettingsDatabase;

namespace MCM.Adapter.ModLib.Settings.Containers.v1
{
    internal sealed class ModLibSettingsContainer : IGlobalSettingsContainer
    {
        private delegate Dictionary<string, v1SettingsBase> GetAllSettingsDictDelegate();

        private static readonly GetAllSettingsDictDelegate? GetAllSettingsDict =
            AccessTools2.GetDelegate<GetAllSettingsDictDelegate>(AccessTools.Property(typeof(v1SettingsDatabase), "AllSettingsDict").GetMethod);

        private Dictionary<string, ModLibGlobalSettingsWrapper> LoadedModLibSettings { get; } = new();

        public List<SettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                ReloadAll();

                return LoadedModLibSettings.Keys
                    .Select(id => new SettingsDefinition(id))
                    .OrderByDescending(a => a.DisplayName)
                    .ToList();
            }
        }

        public BaseSettings? GetSettings(string id)
        {
            Reload(id);
            return LoadedModLibSettings.TryGetValue(id, out var settings) ? settings : null;
        }
        public bool SaveSettings(BaseSettings settings)
        {
            if (settings is ModLibGlobalSettingsWrapper { Object: v1SettingsBase modLibSettings })
            {
                v1SettingsDatabase.SaveSettings(modLibSettings);
                return true;
            }

            return false;
        }

        public bool OverrideSettings(BaseSettings newSettings)
        {
            if (newSettings is ModLibGlobalSettingsWrapper)
            {
                SettingsUtils.OverrideSettings(LoadedModLibSettings[newSettings.Id], newSettings);
                return true;
            }

            return false;
        }
        public bool ResetSettings(BaseSettings settings)
        {
            if (settings is ModLibGlobalSettingsWrapper)
            {
                SettingsUtils.ResetSettings(LoadedModLibSettings[settings.Id]);
                return true;
            }

            return false;
        }

        private void ReloadAll()
        {
            foreach (var settings in GetAllSettingsDict?.Invoke().Values ?? Enumerable.Empty<v1SettingsBase>())
            {
                var id = settings.ID;
                if (!LoadedModLibSettings.ContainsKey(id))
                    LoadedModLibSettings.Add(id, new ModLibGlobalSettingsWrapper(settings));
                else
                    LoadedModLibSettings[id].UpdateReference(settings);
            }
        }
        private void Reload(string id)
        {
            var dict = GetAllSettingsDict?.Invoke();
            if (dict?.ContainsKey(id) == true)
                LoadedModLibSettings[id].UpdateReference(dict[id]);
        }
    }
}