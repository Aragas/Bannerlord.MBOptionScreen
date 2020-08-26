extern alias v13;

using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;
using MCM.Implementation.ModLib.Settings.Base.v13;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.ModLib.Settings.Containers.v13
{
    internal sealed class ModLibDefinitionsSettingsContainer : IModLibDefinitionsSettingsContainer
    {
        private delegate Dictionary<string, v13::ModLib.Definitions.SettingsBase> GetAllSettingsDictDelegate();
        private delegate bool SaveSettingsDelegate(v13::ModLib.Definitions.SettingsBase settings);

        private static readonly GetAllSettingsDictDelegate GetAllSettingsDict =
            AccessTools2.GetDelegate<GetAllSettingsDictDelegate>(AccessTools.Property(typeof(v13::ModLib.Definitions.SettingsDatabase), "AllSettingsDict").GetMethod)!;
        private static readonly SaveSettingsDelegate SaveSettingsFunc =
            AccessTools2.GetDelegate<SaveSettingsDelegate>(typeof(v13::ModLib.Definitions.SettingsDatabase), "SaveSettings")!;

        private Dictionary<string, ModLibDefinitionsGlobalSettingsWrapper> LoadedModLibSettings { get; } = new Dictionary<string, ModLibDefinitionsGlobalSettingsWrapper>();

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
            if (settings is ModLibDefinitionsGlobalSettingsWrapper settingsWrapper && settingsWrapper.Object is v13::ModLib.Definitions.SettingsBase modLibSettings)
            {
                SaveSettingsFunc(modLibSettings);
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
            foreach (var settings in GetAllSettingsDict().Values)
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
            var dict = GetAllSettingsDict();
            if (dict.ContainsKey(id))
                LoadedModLibSettings[id].UpdateReference(dict[id]);
        }
    }
}