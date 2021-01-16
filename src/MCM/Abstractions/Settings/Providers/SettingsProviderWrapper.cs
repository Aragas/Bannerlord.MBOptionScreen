using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Providers
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class SettingsProviderWrapper : BaseSettingsProvider, IWrapper
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public SettingsProviderWrapper(object @object) { }

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions => Enumerable.Empty<SettingsDefinition>();
        public override BaseSettings? GetSettings(string id) => null;
        public override void SaveSettings(BaseSettings settings) { }
        public override void ResetSettings(BaseSettings settings) { }
        public override void OverrideSettings(BaseSettings settings) { }
        public override void OnGameStarted(Game game) { }
        public override void OnGameEnded(Game game) { }
    }
}