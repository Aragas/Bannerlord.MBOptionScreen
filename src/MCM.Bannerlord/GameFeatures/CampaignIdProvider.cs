using MCM.Abstractions.GameFeatures;

using TaleWorlds.CampaignSystem;

namespace MCM.Internal.GameFeatures
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class CampaignIdProvider : ICampaignIdProvider
    {
        public string? GetCurrentCampaignId() => Campaign.Current?.UniqueGameId;
    }
}