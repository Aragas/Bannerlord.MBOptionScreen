using MCM.Abstractions.Settings.Base.PerCharacter;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCharacter
{
    public abstract class FluentPerCharacterSettingsContainerWrapper : BaseSettingsContainerWrapper, IFluentPerCharacterSettingsContainer
    {
        public override bool IsCorrect { get; }

        protected FluentPerCharacterSettingsContainerWrapper(object @object) : base(@object) { }

        public void OnGameStarted(Game game) { }
        public void OnGameEnded(Game game) { }
        public void Register(FluentPerCharacterSettings settings) { }
        public void Unregister(FluentPerCharacterSettings settings) { }
    }
}