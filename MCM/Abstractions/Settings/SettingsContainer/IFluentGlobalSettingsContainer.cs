namespace MCM.Abstractions.Settings.SettingsContainer
{
    public interface IFluentGlobalSettingsContainer : IGlobalSettingsContainer, IDependency
    {
        void Register(FluentGlobalSettings settings);
        void Unregister(FluentGlobalSettings settings);
    }
}