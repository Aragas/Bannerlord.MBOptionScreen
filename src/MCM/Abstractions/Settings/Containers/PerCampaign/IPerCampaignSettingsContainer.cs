using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCampaign
{
    public interface IPerCampaignSettingsContainer : ISettingsContainer, IDependencyBase
    {
        void OnGameStarted(Game game);
        void OnGameEnded(Game game);
    }
}