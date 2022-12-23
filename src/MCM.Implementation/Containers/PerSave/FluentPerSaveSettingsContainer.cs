using BUTR.DependencyInjection;

using MCM.Abstractions.Base;
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

        /// <inheritdoc/>
        public void Register(FluentPerSaveSettings settings)
        {
            RegisterSettings(settings);
        }

        /// <inheritdoc/>
        public void Unregister(FluentPerSaveSettings settings)
        {
            if (LoadedSettings.ContainsKey(settings.Id))
                LoadedSettings.Remove(settings.Id);
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(FluentPerSaveSettings? perSaveSettings)
        {
            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return;

            var behavior = GenericServiceProvider.GetService<IPerSaveSettingsProvider>();
            if (behavior is null)
                return;

            if (perSaveSettings is null)
                return;

            LoadedSettings.Add(perSaveSettings.Id, perSaveSettings);

            behavior.LoadSettings(perSaveSettings);
                perSaveSettings.OnPropertyChanged(BaseSettings.LoadingComplete);
        }

        /// <inheritdoc/>
        public override bool SaveSettings(BaseSettings settings)
        {
            if (GenericServiceProvider.GameScopeServiceProvider is null)
                return false;

            var behavior = GenericServiceProvider.GetService<IPerSaveSettingsProvider>();
            if (behavior is null)
                return false;

            if (settings is not PerSaveSettings saveSettings || !LoadedSettings.ContainsKey(saveSettings.Id))
                return false;

            behavior.SaveSettings(saveSettings);

            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
            return true;
        }

        /// <inheritdoc/>
        public void LoadSettings() { }

        public void OnGameStarted() => LoadedSettings.Clear();
        public void OnGameEnded() => LoadedSettings.Clear();
    }
}