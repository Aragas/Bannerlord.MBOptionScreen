using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerSave
{
    public interface IPerSaveSettingsContainer : ISettingsContainer
    {
        void OnGameStarted(Game game);
        void OnGameEnded(Game game);
    }
}