using BUTR.DependencyInjection;

using MCM.Abstractions.Base;
using MCM.Abstractions.Base.PerSave;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerSave;

namespace MCM.Implementation.PerSave
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class FluentPerSaveSettingsContainer : BaseSettingsContainer<FluentPerSaveSettings>, IFluentPerSaveSettingsContainer
    {
        private readonly IGameEventListener _gameEventListener;

        public FluentPerSaveSettingsContainer(IGameEventListener gameEventListener)
        {
            _gameEventListener = gameEventListener;
            _gameEventListener.GameStarted += GameStarted;
            _gameEventListener.GameEnded += GameEnded;
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

        public void GameStarted() => LoadedSettings.Clear();
        public void GameEnded() => LoadedSettings.Clear();
    }
}