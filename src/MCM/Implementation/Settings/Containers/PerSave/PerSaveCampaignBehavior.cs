using MCM.Abstractions.Settings.Base.PerSave;
using MCM.DependencyInjection;
using MCM.Implementation.Settings.Formats.Json2;

using System.Collections.Generic;
using System.IO;

using TaleWorlds.CampaignSystem;

namespace MCM.Implementation.Settings.Containers.PerSave
{
    internal class PerSaveCampaignBehavior : CampaignBehaviorBase
    {
        private Dictionary<string, string>? _settings = new();

        public Dictionary<string, string>? Settings => _settings;

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_settings", ref _settings);

            if (dataStore.IsLoading && _settings is null)
            {
                _settings = new Dictionary<string, string>();
            }
        }

        public override void RegisterEvents() { }

        public bool SaveSettings(PerSaveSettings perSaveSettings)
        {
            var jsonSettingsFormat = GenericServiceProvider.GetService<IJson2SettingsFormat>();

            if (_settings is null || jsonSettingsFormat is null)
                return false;

            var key = $"{Path.Combine(perSaveSettings.FolderName, perSaveSettings.SubFolder, perSaveSettings.Id)}";
            _settings[key] = jsonSettingsFormat.SaveJson(perSaveSettings);
            return true;
        }

        public void LoadSettings(PerSaveSettings perSaveSettings)
        {
            var jsonSettingsFormat = GenericServiceProvider.GetService<IJson2SettingsFormat>();

            if (_settings is null || jsonSettingsFormat is null)
                return;

            var key = $"{Path.Combine(perSaveSettings.FolderName, perSaveSettings.SubFolder, perSaveSettings.Id)}";
            if (_settings.TryGetValue(key, out var jsonData))
                jsonSettingsFormat.LoadFromJson(perSaveSettings, jsonData);
        }
    }
}