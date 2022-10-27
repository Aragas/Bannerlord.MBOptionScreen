using MCM.Abstractions.Base.PerSave;

namespace MCM.Abstractions.PerSave
{
    public interface IFluentPerSaveSettingsContainer : IPerSaveSettingsContainer
    {
        void Register(FluentPerSaveSettings settings);
        void Unregister(FluentPerSaveSettings settings);
    }
}