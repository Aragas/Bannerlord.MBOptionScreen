using MCM.Abstractions.GameFeatures;

using TaleWorlds.CampaignSystem;

namespace MCM.Internal.GameFeatures
{
    internal sealed class CampaignIdProvider : ICampaignIdProvider
    {
        public string? GetCurrentCampaignId() => Campaign.Current?.UniqueGameId;
    }
}