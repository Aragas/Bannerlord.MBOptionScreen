using MCM.Abstractions.Settings.Base.PerCampaign;

namespace MCM.Implementation.Settings.Base.PerCampaign
{
    public abstract class BaseMCMPerCampaignSettingsWrapper : BasePerCampaignSettingsWrapper
    {
        protected BaseMCMPerCampaignSettingsWrapper(object @object) : base(@object) { }
    }
}