namespace MCM.Abstractions.PerSave
{
    public interface IPerSaveSettingsContainer : ISettingsContainer, ISettingsContainerHasSettingsDefinitions, ISettingsContainerCanOverride, ISettingsContainerCanReset
    {
        void LoadSettings();
    }
}