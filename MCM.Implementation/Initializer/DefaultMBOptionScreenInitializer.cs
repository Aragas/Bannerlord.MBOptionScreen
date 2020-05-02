using HarmonyLib;

using MCM.Abstractions;
using MCM.Abstractions.ApplicationContainer;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;
using MCM.Abstractions.Initializer;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

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
    public sealed class DefaultMBOptionScreenInitializer : IMBOptionScreenInitializer
    {
        private ApplicationVersion GameVersion { get; set; }
        private IApplicationContainerProvider ApplicationContainerProvider { get; set; }

        public void StartInitialization(ApplicationVersion gameVersion, bool first)
        {
            GameVersion = gameVersion;

            ApplicationContainerProvider = DI.GetImplementation<IApplicationContainerProvider, ApplicationContainerProviderWrapper>(GameVersion);
            if (first)
            {
                var settingsProvider = DI.GetImplementation<IMBOptionScreenSettingsContainer, SettingsContainerWrapper>(GameVersion);
                var modLibSettingsProvider = DI.GetImplementation<IModLibSettingsContainer, SettingsContainerWrapper>(GameVersion);
                ApplicationContainerProvider.Set("MBOptionScreenSettingsProvider", settingsProvider);
                ApplicationContainerProvider.Set("ModLibSettingsProvider", modLibSettingsProvider);

                Resolver.GameMenuScreenHandler.AddScreen(
                    "ModOptionsMenu_MBOptionScreen_v3",
                    9990,
                    () => (ScreenBase) DI.GetImplementation(GameVersion, typeof(MBOptionScreen).FullName),
                    new TextObject("{=HiZbHGvYG}Mod Options"));


                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"));
                foreach (var assembly in assemblies)
                {
                    var settingsProviderWrapperType = assembly.GetType("MBOptionScreen.Settings.SettingsProviderWrapper");
                    var settingsDatabaseType = assembly.GetType("MBOptionScreen.Settings.SettingsDatabase");
                    var mbOptionScreenSettingsProviderProperty =
                        AccessTools.Property(settingsDatabaseType, "MBOptionScreenSettingsProvider");
                    var modLibSettingsProviderProperty =
                        AccessTools.Property(settingsDatabaseType, "ModLibSettingsProvider");
                    mbOptionScreenSettingsProviderProperty?.SetValue(
                        null,
                        Activator.CreateInstance(settingsProviderWrapperType, new object[] { settingsProvider }));
                    modLibSettingsProviderProperty?.SetValue(
                        null,
                        Activator.CreateInstance(settingsProviderWrapperType, new object[] { modLibSettingsProvider }));
                }

                RegisterSettings();
            }
        }
        public void EndInitialization(bool first)
        {
            if (first)
            {
                Resolver.IngameMenuScreenHandler.AddScreen(
                    1,
                    () => (ScreenBase) DI.GetImplementation(GameVersion, typeof(MBOptionScreen).FullName),
                    new TextObject("{=NqarFr4P}Mod Options", null));

                if (MCMSettings.Instance!.OverrideModLib)
                    Resolver.ModLibScreenOverrider.OverrideModLibScreen();


                ApplicationContainerProvider.Clear();
            }
        }

        /// <summary>
        /// Find all implementations of SettingBase and register them
        /// </summary>
        private static void RegisterSettings()
        {
            var settings = new List<SettingsBase>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => !a.FullName.StartsWith("System"))
                .Where(a => !a.FullName.StartsWith("Microsoft"))
                .Where(a => !a.FullName.StartsWith("mscorlib"))
                // ignore v1 and v2 classes
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            // ModLib
            var modLibSettings = allTypes
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, "ModLib.SettingsBase"))
                .Select(obj => new ModLibSettingsWrapper(Activator.CreateInstance(obj)));
            settings.AddRange(modLibSettings);

            var mbOptionScreenSettings = allTypes
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MBOptionScreen.Settings.SettingsBase") ||
                            ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(SettingsBase)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(EmptySettings)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(SettingsWrapper)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(ModLibSettingsWrapper)))
#if !DEBUG
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TestSettingsBase<>)))
#endif
                .Select(obj => new SettingsWrapper(Activator.CreateInstance(obj)));
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
                BaseSettingsProvider.Instance.RegisterSettings(setting);
        }
    }
}