using HarmonyLib;

using MCM.Abstractions.ApplicationContainer;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;
using MCM.Abstractions.Initializer;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Utils;

using System;
using System.IO;
using System.Linq;
using MCM.Implementation.Settings.SettingsContainer;
using TaleWorlds.Library;

namespace MCM.Implementation.Initializer
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
    internal sealed class DefaultMBOptionScreenInitializer : IMBOptionScreenInitializer
    {
        private ApplicationVersion GameVersion { get; set; }
        private IApplicationContainerProvider ApplicationContainerProvider { get; set; }

        public void StartInitialization(ApplicationVersion gameVersion, bool first)
        {
            GameVersion = gameVersion;

            ApplicationContainerProvider = DI.GetImplementation<IApplicationContainerProvider, ApplicationContainerProviderWrapper>(GameVersion)!;
            if (first)
            {
                var settingsProvider = DI.GetImplementation<IGlobalSettingsContainer, SettingsContainerWrapper>(GameVersion)!;
                var modLibSettingsProvider = DI.GetImplementation<IModLibSettingsContainer, ModLibSettingsContainerWrapper>(GameVersion)!;
                ApplicationContainerProvider.Set("MBOptionScreenSettingsProvider", settingsProvider);
                ApplicationContainerProvider.Set("ModLibSettingsProvider", modLibSettingsProvider);

                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"));
                foreach (var assembly in assemblies)
                {
                    var settingsProviderWrapperType = assembly.GetType("MBOptionScreen.Settings.SettingsProviderWrapper");
                    var settingsDatabaseType = assembly.GetType("MBOptionScreen.Settings.SettingsDatabase");
                    var mbOptionScreenSettingsProviderProperty = AccessTools.Property(settingsDatabaseType, "MBOptionScreenSettingsProvider");
                    var modLibSettingsProviderProperty = AccessTools.Property(settingsDatabaseType, "ModLibSettingsProvider");
                    mbOptionScreenSettingsProviderProperty?.SetValue(
                        null,
                        Activator.CreateInstance(settingsProviderWrapperType, new object[] { settingsProvider }));
                    modLibSettingsProviderProperty?.SetValue(
                        null,
                        Activator.CreateInstance(settingsProviderWrapperType, new object[] { modLibSettingsProvider }));
                }
            }
        }
        public void EndInitialization(bool first)
        {
            if (first)
            {
                if (MCMSettings.Instance!.OverrideModLib)
                    Resolver.ModLibScreenOverrider.OverrideModLibScreen();

                ApplicationContainerProvider.Clear();
            }
        }
    }
}