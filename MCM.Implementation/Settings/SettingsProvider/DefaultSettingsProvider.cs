using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.SettingsProvider
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    internal sealed class DefaultSettingsProvider : BaseSettingsProvider
    {
        private List<ISettingsContainer> SettingsContainers { get; }

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions => 
            SettingsContainers.SelectMany(sp => sp.CreateModSettingsDefinitions)
                .OrderByDescending(x => x.DisplayName, new AlphanumComparatorFast());

        public DefaultSettingsProvider()
        {
            SettingsContainers = DI.GetImplementations<ISettingsContainer, SettingsContainerWrapper>(ApplicationVersionUtils.GameVersion()).ToList();
        }

        public override BaseSettings? GetSettings(string id)
        {
            foreach (var settingsContainer in SettingsContainers)
            {
                if (settingsContainer.GetSettings(id) is {} settings)
                    return settings is BaseSettings baseSettings ? baseSettings : BaseGlobalSettingsWrapper.Create(settings);
            }
            return null;
        }
        public override void SaveSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in SettingsContainers)
                settingsContainer.SaveSettings(settings);
            settings.OnPropertyChanged(BaseSettings.SaveTriggered);
        }

        public override void ResetSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in SettingsContainers)
                settingsContainer.ResetSettings(settings);
        }
        public override void OverrideSettings(BaseSettings settings)
        {
            foreach (var settingsContainer in SettingsContainers)
                settingsContainer.OverrideSettings(settings);
        }

        public override void OnGameStarted(Game game)
        {
            foreach (var settingsContainer in SettingsContainers)
            {
                if (settingsContainer is IPerCharacterContainer perCharacterContainer)
                {
                    perCharacterContainer.OnGameStarted(game);
                }
            }
        }
        public override void OnGameEnded(Game game)
        {
            foreach (var settingsContainer in SettingsContainers)
            {
                if (settingsContainer is IPerCharacterContainer perCharacterContainer)
                {
                    perCharacterContainer.OnGameEnded(game);

                }
            }
        }
    }
}