using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base.PerSave;
using MCM.Implementation.Settings.Formats.Json2;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.IO;

using TaleWorlds.CampaignSystem;

namespace MCM.Implementation.Settings.Containers.PerSave
{
    internal class PerSaveCampaignBehavior : CampaignBehaviorBase
    {
        private Dictionary<string, string>? _settings = new Dictionary<string, string>();

        public Dictionary<string, string>? Settings => _settings;

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_settings", ref _settings);

            if (dataStore.IsLoading && _settings == null)
            {
                _settings = new Dictionary<string, string>();
            }
        }

        public override void RegisterEvents() { }

        public bool SaveSettings(PerSaveSettings perSaveSettings)
        {
            var jsonSettingsFormat = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IJson2SettingsFormat>();

            if (_settings == null || jsonSettingsFormat == null)
                return false;

            var key = $"{Path.Combine(perSaveSettings.FolderName, perSaveSettings.SubFolder, perSaveSettings.Id)}";
            _settings[key] = jsonSettingsFormat.SaveJson(perSaveSettings);
            return true;
        }

        public void LoadSettings(PerSaveSettings perSaveSettings)
        {
            var jsonSettingsFormat = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IJson2SettingsFormat>();

            if (_settings == null || jsonSettingsFormat == null)
                return;

            var key = $"{Path.Combine(perSaveSettings.FolderName, perSaveSettings.SubFolder, perSaveSettings.Id)}";
            if (_settings.TryGetValue(key, out var jsonData))
                jsonSettingsFormat.LoadFromJson(perSaveSettings, jsonData);
        }
    }
}