namespace MCM.Abstractions.Settings.Containers.Global
{
    public interface IGlobalSettingsContainer :
        ISettingsContainer,
        ISettingsContainerHasSettingsDefinitions,
        ISettingsContainerCanOverride,
        ISettingsContainerCanReset,
        ISettingsContainerPresets
    {
        
    }
}