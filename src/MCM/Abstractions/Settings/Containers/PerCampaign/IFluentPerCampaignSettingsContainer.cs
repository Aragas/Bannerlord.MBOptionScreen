using MCM.Abstractions.Settings.Base.PerCampaign;

namespace MCM.Abstractions.Settings.Containers.PerCampaign
{
    public interface IFluentPerCampaignSettingsContainer : IPerCampaignSettingsContainer
    {
        void Register(FluentPerCampaignSettings settings);
        void Unregister(FluentPerCampaignSettings settings);
    }
}