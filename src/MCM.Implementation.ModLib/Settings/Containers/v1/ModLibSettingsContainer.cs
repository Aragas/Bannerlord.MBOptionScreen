extern alias v1;

using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Implementation.ModLib.Settings.Base.v1;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.ModLib.Settings.Containers.v1
{
    internal sealed class ModLibSettingsContainer : IGlobalSettingsContainer
    {
        private delegate Dictionary<string, v1::ModLib.SettingsBase> GetAllSettingsDictDelegate();

        private static readonly GetAllSettingsDictDelegate GetAllSettingsDict =
            AccessTools2.GetDelegate<GetAllSettingsDictDelegate>(AccessTools.Property(typeof(v1::ModLib.SettingsDatabase), "AllSettingsDict").GetMethod)!;

        private Dictionary<string, ModLibGlobalSettingsWrapper> LoadedModLibSettings { get; } = new Dictionary<string, ModLibGlobalSettingsWrapper>();

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
            if (settings is ModLibGlobalSettingsWrapper settingsWrapper && settingsWrapper.Object is v1::ModLib.SettingsBase modLibSettings)
            {
                v1::ModLib.SettingsDatabase.SaveSettings(modLibSettings);
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
            foreach (var settings in GetAllSettingsDict().Values)
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
            var dict = GetAllSettingsDict();
            if (dict.ContainsKey(id))
                LoadedModLibSettings[id].UpdateReference(dict[id]);
        }
    }
}