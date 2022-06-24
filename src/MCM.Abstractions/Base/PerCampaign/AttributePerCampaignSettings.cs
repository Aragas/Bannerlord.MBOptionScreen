namespace MCM.Abstractions.Base.PerCampaign
{
    public abstract class AttributePerCampaignSettings<T> : PerCampaignSettings<T> where T : PerCampaignSettings, new()
    {
        public sealed override string DiscoveryType => "attributes";
    }
}