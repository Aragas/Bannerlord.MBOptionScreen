namespace MCM.Abstractions.Settings.Containers.PerSave
{
    public interface IPerSaveSettingsContainer : ISettingsContainer, ISettingsContainerHasSettingsDefinitions, ISettingsContainerCanOverride, ISettingsContainerCanReset
    {
        void LoadSettings();
    }
}