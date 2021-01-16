using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCharacter
{
    public abstract class BasePerCharacterSettingsContainerWrapper : BaseSettingsContainerWrapper, IPerCharacterSettingsContainer
    {
        public override bool IsCorrect { get; }

        protected BasePerCharacterSettingsContainerWrapper(object @object) : base(@object) { }

        public void OnGameStarted(Game game) { }
        public void OnGameEnded(Game game) { }
    }
}