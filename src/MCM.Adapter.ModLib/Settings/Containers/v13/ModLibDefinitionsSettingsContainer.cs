extern alias v13;

using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Adapter.ModLib.Settings.Base.v13;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

using v13SettingsBase = v13::ModLib.Definitions.SettingsBase;
using v13SettingsDatabase = v13::ModLib.Definitions.SettingsDatabase;

namespace MCM.Adapter.ModLib.Settings.Containers.v13
{
    internal sealed class ModLibDefinitionsSettingsContainer : IGlobalSettingsContainer
    {
        private delegate Dictionary<string, v13SettingsBase> GetAllSettingsDictDelegate();
        private delegate bool SaveSettingsDelegate(v13SettingsBase settings);

        private static readonly GetAllSettingsDictDelegate? GetAllSettingsDict =
            AccessTools2.GetDelegate<GetAllSettingsDictDelegate>(AccessTools.Property(typeof(v13SettingsDatabase), "AllSettingsDict").GetMethod);
        private static readonly SaveSettingsDelegate? SaveSettingsFunc =
            AccessTools2.GetDelegate<SaveSettingsDelegate>(typeof(v13SettingsDatabase), "SaveSettings");

        private Dictionary<string, ModLibDefinitionsGlobalSettingsWrapper> LoadedModLibSettings { get; } = new();

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
            if (settings is ModLibDefinitionsGlobalSettingsWrapper { Object: v13SettingsBase modLibSettings })
            {
                SaveSettingsFunc?.Invoke(modLibSettings);
                return true;
            }

            return false;
        }

        public bool OverrideSettings(BaseSettings newSettings)
        {
            if (newSettings is ModLibDefinitionsGlobalSettingsWrapper)
            {
                SettingsUtils.OverrideSettings(LoadedModLibSettings[newSettings.Id], newSettings);
                return true;
            }

            return false;
        }
        public bool ResetSettings(BaseSettings settings)
        {
            if (settings is ModLibDefinitionsGlobalSettingsWrapper)
            {
                SettingsUtils.ResetSettings(LoadedModLibSettings[settings.Id]);
                return true;
            }

            return false;
        }


        private void ReloadAll()
        {
            foreach (var settings in GetAllSettingsDict?.Invoke().Values ?? Enumerable.Empty<v13SettingsBase>())
            {
                var id = settings.ID;
                if (!LoadedModLibSettings.ContainsKey(id))
                    LoadedModLibSettings.Add(id, new ModLibDefinitionsGlobalSettingsWrapper(settings));
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