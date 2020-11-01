extern alias v2;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Implementation.MBO.Settings.Base;
using MCM.Implementation.MBO.Settings.Containers;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

using v2::MBOptionScreen.Interfaces;
using v2::MBOptionScreen.Settings;
using v4::MCM.Abstractions.Settings.Providers;

namespace MCM.Implementation.MBO.Settings.Providers
{
    internal class MBOv2SettingsProvider : IMBOptionScreenSettingsProvider
    {
        List<ModSettingsDefinition> ISettingsProvider.CreateModSettingsDefinitions { get; } = default!;

        public SettingsBase? GetSettings(string id)
        {
            if (v4::MCM.MCMSubModule.Instance is { } && v4::MCM.MCMSubModule.Instance.GetServiceProvider() is { } serviceProvider)
            {
                var settingsProvider = serviceProvider.GetRequiredService<BaseSettingsProvider>();

                var baseSettings = settingsProvider.GetSettings(id);
                if (baseSettings is MBOv2GlobalSettingsWrapper settingsWrapper && settingsWrapper.Object is SettingsBase settings)
                    return settings;
            }
            else
            {
                var container = new MBOv2GlobalSettingsContainer();
                var baseSettings = container.GetSettings(id);
                if (baseSettings is MBOv2GlobalSettingsWrapper settingsWrapper && settingsWrapper.Object is SettingsBase settings)
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