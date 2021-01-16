using MCM.Abstractions.Settings.Base.PerCharacter;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCharacter
{
    public abstract class BasePerCharacterSettingsContainer : BaseSettingsContainer<PerCharacterSettings>, IPerCharacterSettingsContainer
    {
        protected BasePerCharacterSettingsContainer() { }

        public abstract void OnGameStarted(Game game);
        public abstract void OnGameEnded(Game game);
    }
}