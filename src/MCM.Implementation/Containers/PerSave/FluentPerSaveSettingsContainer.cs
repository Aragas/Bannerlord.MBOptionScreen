using MCM.Abstractions;
using MCM.Abstractions.Base.PerSave;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerSave;

namespace MCM.Implementation.PerSave
{
    internal sealed class FluentPerSaveSettingsContainer : BaseSettingsContainer<FluentPerSaveSettings>, IFluentPerSaveSettingsContainer
    {
        private readonly IGameEventListener _gameEventListener;

        public FluentPerSaveSettingsContainer(IGameEventListener gameEventListener)
        {
            _gameEventListener = gameEventListener;
            _gameEventListener.OnGameStarted += OnGameStarted;
            _gameEventListener.OnGameEnded += OnGameEnded;
        }

        public void Register(FluentPerSaveSettings settings)
        {
            RegisterSettings(settings);
        }
        public void Unregister(FluentPerSaveSettings settings)
        {
            if (LoadedSettings.ContainsKey(settings.Id))
                LoadedSettings.Remove(settings.Id);
        }

        public void OnGameStarted() => LoadedSettings.Clear();
        public void LoadSettings() { }
        public void OnGameEnded() => LoadedSettings.Clear();
    }
}