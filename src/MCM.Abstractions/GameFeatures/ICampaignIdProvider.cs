namespace MCM.Abstractions.GameFeatures
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ICampaignIdProvider
    {
        public string? GetCurrentCampaignId();
    }
}