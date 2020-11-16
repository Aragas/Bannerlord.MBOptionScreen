extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Adapter.MCMv3.Settings.Base;
using MCM.Adapter.MCMv3.Settings.Containers;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

using TaleWorlds.Core;

using v4::MCM.Abstractions.Settings.Providers;

using MCMv3BaseSettings = v3::MCM.Abstractions.Settings.Base.BaseSettings;
using MCMv3BaseSettingsProvider = v3::MCM.Abstractions.Settings.Providers.BaseSettingsProvider;
using MCMv3IWrappers = v3::MCM.Abstractions.IWrapper;
using MCMv3SettingsDefinition = v3::MCM.Abstractions.Settings.Models.SettingsDefinition;

namespace MCM.Adapter.MCMv3.Settings.Providers
{
    /// <summary>
    /// Replaces MCMv3's default <see cref="MCMv3BaseSettingsProvider"/> with MCMv4's one.
    /// </summary>
    internal sealed class MCMv3SettingsProviderReplacer : MCMv3BaseSettingsProvider
    {
        public override IEnumerable<MCMv3SettingsDefinition> CreateModSettingsDefinitions { get; } = default!;

        public override MCMv3BaseSettings? GetSettings(string id)
        {
            if (v4::MCM.MCMSubModule.Instance is not null && v4::MCM.MCMSubModule.Instance.GetServiceProvider() is { } serviceProvider)
            {
                var settingsProvider = serviceProvider.GetRequiredService<BaseSettingsProvider>();

                var baseSettings = settingsProvider.GetSettings(id);
                if (baseSettings is MCMv3GlobalSettingsWrapper settingsWrapper && settingsWrapper.Object is MCMv3BaseSettings settings)
                    return settings;
            }
            else
            {
                var container = new MCMv3GlobalSettingsContainer();
                var baseSettings = container.GetSettings(id);
                if (baseSettings is MCMv3GlobalSettingsWrapper settingsWrapper && settingsWrapper.Object is MCMv3BaseSettings settings)
                    return settings;
            }

            return null;
        }
        public override object? GetSettingsObject(string id)
        {
            var settings = GetSettings(id);
            return settings switch
            {
                MCMv3IWrappers wrapper => wrapper.Object,
                _ => settings
            };
        }

        public override void SaveSettings(MCMv3BaseSettings settings) { }
        public override void ResetSettings(MCMv3BaseSettings settings) { }
        public override void OverrideSettings(MCMv3BaseSettings settings) { }

        public override void OnGameStarted(Game game) { }
        public override void OnGameEnded(Game game) { }
    }
}