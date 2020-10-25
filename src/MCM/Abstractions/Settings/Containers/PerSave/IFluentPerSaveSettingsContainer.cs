using MCM.Abstractions.Settings.Base.PerSave;

namespace MCM.Abstractions.Settings.Containers.PerSave
{
    public interface IFluentPerSaveSettingsContainer : IPerSaveSettingsContainer
    {
        void Register(FluentPerSaveSettings settings);
        void Unregister(FluentPerSaveSettings settings);
    }
}