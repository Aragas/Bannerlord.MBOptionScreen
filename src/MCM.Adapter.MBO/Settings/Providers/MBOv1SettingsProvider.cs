extern alias v1;
extern alias v4;

using MCM.Adapter.MBO.Settings.Base;
using MCM.Adapter.MBO.Settings.Containers;

using System.Collections.Generic;

using v4::MCM;
using v4::MCM.Abstractions.Settings.Providers;
using v4::MCM.DependencyInjection;

using ISettingsProvider = v1::MBOptionScreen.Interfaces.ISettingsProvider;
using ModSettingsDefinition = v1::MBOptionScreen.Settings.ModSettingsDefinition;
using SettingsBase = v1::MBOptionScreen.Settings.SettingsBase;

namespace MCM.Adapter.MBO.Settings.Providers
{
    internal sealed class MBOv1SettingsProvider : ISettingsProvider
    {
        List<ModSettingsDefinition> ISettingsProvider.CreateModSettingsDefinitions { get; } = default!;

        public SettingsBase? GetSettings(string id)
        {
            if (MCMSubModule.Instance is not null && GenericServiceProvider.GetService<BaseSettingsProvider>() is { } settingsProvider)
            {
                var baseSettings = settingsProvider.GetSettings(id);
                if (baseSettings is MBOv1GlobalSettingsWrapper { Object: SettingsBase settings })
                    return settings;
            }
            else
            {
                var container = new MBOv1GlobalSettingsContainer();
                var baseSettings = container.GetSettings(id);
                if (baseSettings is MBOv1GlobalSettingsWrapper { Object: SettingsBase settings })
                    return settings;
            }

            return null;
        }

        bool ISettingsProvider.RegisterSettings(SettingsBase settingsClass) => true;

        void ISettingsProvider.SaveSettings(SettingsBase settingsInstance) { }
        SettingsBase? ISettingsProvider.ResetSettings(string id) => null;
        bool ISettingsProvider.OverrideSettings(SettingsBase settings) => true;
    }
}