using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public interface IPerCharacterContainer : ISettingsContainer
    {
        void OnGameStarted(Game game);
        void OnGameEnded(Game game);
    }
}