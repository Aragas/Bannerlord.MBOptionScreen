using MCM.Abstractions.Base.PerCampaign;

namespace MCMv5.Tests;

public abstract class BaseTestPerCampaignSettingsBase<T> : AttributePerCampaignSettings<T> where T : PerCampaignSettings, new()
{
    public override string FolderName => "MCMv5.Tests";
}