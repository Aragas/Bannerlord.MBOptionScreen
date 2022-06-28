using MCM.Abstractions.Base.PerCampaign;

namespace MCM.Abstractions.PerCampaign
{
    public interface IFluentPerCampaignSettingsContainer : IPerCampaignSettingsContainer
    {
        void Register(FluentPerCampaignSettings settings);
        void Unregister(FluentPerCampaignSettings settings);
    }
}