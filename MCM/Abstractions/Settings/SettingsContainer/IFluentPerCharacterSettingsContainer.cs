namespace MCM.Abstractions.Settings.SettingsContainer
{
    public interface IFluentPerCharacterSettingsContainer : IPerCharacterSettingsContainer, IDependency
    {
        void Register(FluentPerCharacterSettings settings);
        void Unregister(FluentPerCharacterSettings settings);
    }
}