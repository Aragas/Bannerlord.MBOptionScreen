namespace MCM.Abstractions.Global
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