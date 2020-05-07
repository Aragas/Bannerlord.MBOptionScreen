using System.IO;
using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public abstract class BasePerCharacterSettingsContainer : BaseSettingsContainer<PerCharacterSettings>
    {
        protected override string RootFolder { get; }

        protected BasePerCharacterSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "PerCharacter");
        }

        protected override void RegisterSettings(PerCharacterSettings tSettings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return;

            if (tSettings == null || LoadedSettings.ContainsKey(tSettings.Id))
                return;

            LoadedSettings.Add(tSettings.Id, tSettings);

            var path = Path.Combine(RootFolder, tSettings.CharacterId, tSettings.FolderName, tSettings.SubFolder ?? "", $"{tSettings.Id}.{tSettings.Format}");
            if (AvailableSettingsFormats.ContainsKey(tSettings.Format))
                AvailableSettingsFormats[tSettings.Format].Load(tSettings, path);
            else
                AvailableSettingsFormats["memory"].Load(tSettings, path);
        }

        public override bool SaveSettings(BaseSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return false;

            if (!(settings is PerCharacterSettings tSettings) || !LoadedSettings.ContainsKey(tSettings.Id))
                return false;

            var path = Path.Combine(RootFolder, tSettings.CharacterId, tSettings.FolderName, tSettings.SubFolder ?? "", $"{tSettings.Id}.{tSettings.Format}");
            if (AvailableSettingsFormats.ContainsKey(tSettings.Format))
                AvailableSettingsFormats[tSettings.Format].Save(tSettings, path);
            else
                AvailableSettingsFormats["memory"].Save(tSettings, path);

            return true;
        }

        public override bool ResetSettings(BaseSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return false;

            return base.ResetSettings(settings);
        }
        public override bool OverrideSettings(BaseSettings settings)
        {
            if (Game.Current?.PlayerTroop?.StringId == null)
                return false;

            return base.OverrideSettings(settings);
        }
    }
}