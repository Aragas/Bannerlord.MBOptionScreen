namespace MCM.Abstractions.Global
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IGlobalSettingsContainer :
        ISettingsContainer,
        ISettingsContainerHasSettingsDefinitions,
        ISettingsContainerCanOverride,
        ISettingsContainerCanReset,
        ISettingsContainerPresets
    {

    }
}