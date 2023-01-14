using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Base;
using MCM.Abstractions.Base.PerSave;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.PerSave;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.PerSave
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class PerSaveSettingsContainer : BaseSettingsContainer<PerSaveSettings>, IPerSaveSettingsContainer
    {
        private readonly IBUTRLogger _logger;
        private readonly IGameEventListener _gameEventListener;

        public PerSaveSettingsContainer(IBUTRLogger<PerSaveSettingsContainer> logger, IGameEventListener gameEventListener)
        {
            _logger = logger;
            _gameEventListener = gameEventListener;
            _gameEventListener.GameStarted += GameStarted;
            _gameEventListener.GameEnded += GameEnded;
        }

        /// <inheritdoc/>
        protected override void RegisterSettings(PerSaveSettings? perSaveSettings)
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

        private void GameStarted()
        {
            LoadedSettings.Clear();
        }

        public void LoadSettings()
        {
            IEnumerable<PerSaveSettings> GetPerSaveSettings()
            {
                foreach (var assembly in AccessTools2.AllAssemblies().Where(a => !a.IsDynamic))
                {
                    IEnumerable<PerSaveSettings> settings;
                    try
                    {
                        settings = AccessTools2.GetTypesFromAssemblyIfValid(assembly)
                            .Where(t => t.IsClass && !t.IsAbstract)
                            .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                            .Where(t => typeof(PerSaveSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(EmptyPerSaveSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                            .Select(t => Activator.CreateInstance(t) as PerSaveSettings)
                            .OfType<PerSaveSettings>()
                            .ToList();
                    }
                    catch (TypeLoadException ex)
                    {
                        settings = Array.Empty<PerSaveSettings>();
                        _logger.LogError(ex, $"Error while handling assembly {assembly}!");
                    }

                    foreach (var setting in settings)
                    {
                        yield return setting;
                    }
                }
            }

            foreach (var setting in GetPerSaveSettings())
                RegisterSettings(setting);
        }

        private void GameEnded()
        {
            LoadedSettings.Clear();
        }
    }
}