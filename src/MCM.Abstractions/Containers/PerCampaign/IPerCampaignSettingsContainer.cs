namespace MCM.Abstractions.PerCampaign
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPerCampaignSettingsContainer : ISettingsContainer, ISettingsContainerHasSettingsDefinitions, ISettingsContainerCanOverride, ISettingsContainerCanReset
    { }
}