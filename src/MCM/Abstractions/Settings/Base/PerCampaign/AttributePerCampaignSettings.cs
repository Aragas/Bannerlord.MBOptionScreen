namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public abstract class AttributePerCampaignSettings<T> : PerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        public override string DiscoveryType => "attributes";
    }
}