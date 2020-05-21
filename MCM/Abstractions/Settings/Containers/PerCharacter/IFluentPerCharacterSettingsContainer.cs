using MCM.Abstractions.Settings.Base.PerCharacter;

namespace MCM.Abstractions.Settings.Containers.PerCharacter
{
    public interface IFluentPerCharacterSettingsContainer : IPerCharacterSettingsContainer, IDependency
    {
        void Register(FluentPerCharacterSettings settings);
        void Unregister(FluentPerCharacterSettings settings);
    }
}