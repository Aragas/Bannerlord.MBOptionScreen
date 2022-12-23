using BUTR.DependencyInjection;

using MCM.Abstractions.Base.PerSave;
using MCM.Abstractions.GameFeatures;
using MCM.Implementation;
using MCM.Implementation.PerSave;

using System.Collections.Generic;
using System.IO;

using TaleWorlds.CampaignSystem;

namespace MCM.Internal.GameFeatures
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class PerSaveCampaignBehavior : CampaignBehaviorBase, IPerSaveSettingsProvider
    {
        private Dictionary<string, string>? _settings = new();

        public Dictionary<string, string>? Settings => _settings;

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_settings", ref _settings);

            if (dataStore.IsLoading)
            {
                _settings ??= new Dictionary<string, string>();

                var perSaveSettingsContainer = GenericServiceProvider.GetService<PerSaveSettingsContainer>();
                perSaveSettingsContainer?.LoadSettings();
            }
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnNewGameCreatedEvent);
        }

        private void OnNewGameCreatedEvent(CampaignGameStarter campaignGameStarter)
        {
            _settings = new Dictionary<string, string>();
            var perSaveSettingsContainer = GenericServiceProvider.GetService<PerSaveSettingsContainer>();
            perSaveSettingsContainer?.LoadSettings();
        }

        public bool SaveSettings(PerSaveSettings perSaveSettings)
        {
            var jsonSettingsFormat = GenericServiceProvider.GetService<JsonSettingsFormat>();

            if (_settings is null || jsonSettingsFormat is null)
                return false;

            var key = $"{Path.Combine(perSaveSettings.FolderName, perSaveSettings.SubFolder, perSaveSettings.Id)}";
            _settings[key] = jsonSettingsFormat.SaveJson(perSaveSettings);
            return true;
        }

        public void LoadSettings(PerSaveSettings perSaveSettings)
        {
            var jsonSettingsFormat = GenericServiceProvider.GetService<JsonSettingsFormat>();

            if (_settings is null || jsonSettingsFormat is null)
                return;

            var key = $"{Path.Combine(perSaveSettings.FolderName, perSaveSettings.SubFolder, perSaveSettings.Id)}";
            if (_settings.TryGetValue(key, out var jsonData))
                jsonSettingsFormat.LoadFromJson(perSaveSettings, jsonData);
        }
    }
}