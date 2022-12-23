using MCM.Abstractions.Base.PerCampaign;

namespace MCM.Abstractions.PerCampaign
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IFluentPerCampaignSettingsContainer : IPerCampaignSettingsContainer
    {
        void Register(FluentPerCampaignSettings settings);
        void Unregister(FluentPerCampaignSettings settings);
    }
}