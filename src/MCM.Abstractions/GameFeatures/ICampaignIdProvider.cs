namespace MCM.Abstractions.GameFeatures
{
    public interface ICampaignIdProvider
    {
        public string? GetCurrentCampaignId();
    }
}