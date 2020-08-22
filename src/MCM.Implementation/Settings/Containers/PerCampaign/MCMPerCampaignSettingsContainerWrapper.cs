using MCM.Abstractions.Settings.Containers.PerCampaign;

namespace MCM.Implementation.Settings.Containers.PerCampaign
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MCMPerCampaignSettingsContainerWrapper : BasePerCampaignSettingsContainerWrapper, IMCMPerCampaignSettingsContainer
    {
        public MCMPerCampaignSettingsContainerWrapper(object @object) : base(@object) { }
    }
}