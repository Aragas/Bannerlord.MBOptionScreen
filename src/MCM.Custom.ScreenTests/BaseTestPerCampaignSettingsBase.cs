using MCM.Abstractions.Settings.Base.PerCampaign;

namespace MCM.Custom.ScreenTests
{
    public abstract class BaseTestPerCampaignSettingsBase<T> : AttributePerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        public override string FolderName => "Testing";
    }
}