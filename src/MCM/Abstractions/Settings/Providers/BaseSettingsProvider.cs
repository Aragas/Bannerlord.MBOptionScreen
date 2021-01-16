using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Providers
{
    public abstract class BaseSettingsProvider : IDependency
    {
        private static BaseSettingsProvider? _instance;
        public static BaseSettingsProvider Instance => _instance;

        public abstract IEnumerable<SettingsDefinition> CreateModSettingsDefinitions { get; }
        public abstract BaseSettings? GetSettings(string id);
        public virtual object? GetSettingsObject(string id) => GetSettings(id);
        public abstract void SaveSettings(BaseSettings settings);
        public abstract void ResetSettings(BaseSettings settings);
        public abstract void OverrideSettings(BaseSettings settings);

        public abstract void OnGameStarted(Game game);
        public abstract void OnGameEnded(Game game);
    }
}