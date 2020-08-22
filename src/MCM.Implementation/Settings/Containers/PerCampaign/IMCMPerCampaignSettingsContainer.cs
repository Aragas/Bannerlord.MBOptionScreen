using MCM.Abstractions;
using MCM.Abstractions.Settings.Containers.PerCampaign;

namespace MCM.Implementation.Settings.Containers.PerCampaign
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMPerCampaignSettingsContainer : IPerCampaignSettingsContainer, IDependency { }
}