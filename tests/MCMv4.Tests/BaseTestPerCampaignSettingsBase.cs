using MCM.Abstractions.Settings.Base.PerCampaign;

namespace MCMv4.Tests
{
    public abstract class BaseTestPerCampaignSettingsBase<T> : AttributePerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        public override string FolderName => "MCMv4.Tests";
    }
}