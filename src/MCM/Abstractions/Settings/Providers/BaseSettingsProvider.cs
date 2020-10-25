using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Providers
{
    public abstract class BaseSettingsProvider
    {
        public static BaseSettingsProvider? Instance =>
            MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<BaseSettingsProvider>();

        public abstract IEnumerable<SettingsDefinition> CreateModSettingsDefinitions { get; }
        public abstract BaseSettings? GetSettings(string id);
        public abstract void SaveSettings(BaseSettings settings);
        public abstract void ResetSettings(BaseSettings settings);
        public abstract void OverrideSettings(BaseSettings settings);

        public abstract void OnGameStarted(Game game);
        public abstract void OnGameEnded(Game game);
    }
}