using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

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
    internal sealed class DefaultSettingsProvider : BaseSettingsProvider
    {
        private List<ISettingsContainer> SettingsProviders { get; }

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions => SettingsProviders.SelectMany(sp => sp.CreateModSettingsDefinitions);

        public DefaultSettingsProvider()
        {
            SettingsProviders = DI.GetImplementations<ISettingsContainer, SettingsContainerWrapper>(ApplicationVersionUtils.GameVersion()).ToList();
        }

        public override SettingsBase? GetSettings(string id)
        {
            foreach (var settingsProvider in SettingsProviders)
            {
                if (settingsProvider.GetSettings(id) is {} settings)
                    return settings is SettingsBase settingsBase ? settingsBase : new SettingsWrapper(settings);
            }
            return null;
        }
        public override void RegisterSettings(SettingsBase settings)
        {
            foreach (var settingsProvider in SettingsProviders)
            {
                if (settingsProvider.RegisterSettings(settings is SettingsWrapper wrapper ? wrapper : settings))
                    break;
            }
        }

        public override void SaveSettings(SettingsBase settings)
        {
            foreach (var settingsProvider in SettingsProviders)
                settingsProvider.SaveSettings(settings is SettingsWrapper wrapper ? wrapper : settings);
        }

        public override SettingsBase? ResetSettings(string id)
        {
            foreach (var settingsProvider in SettingsProviders)
            {
                if (settingsProvider.ResetSettings(id) is { } settings)
                    return settings is SettingsBase settingsBase ? settingsBase : new SettingsWrapper(settings);
            }
            return null;
        }

        public override void OverrideSettings(SettingsBase settings)
        {
            foreach (var settingsProvider in SettingsProviders)
                settingsProvider.OverrideSettings(settings is SettingsWrapper wrapper ? wrapper : settings);
        }
    }
}