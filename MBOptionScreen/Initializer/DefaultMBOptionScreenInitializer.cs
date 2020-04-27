using MBOptionScreen.ApplicationContainer;
using MBOptionScreen.Attributes;
using MBOptionScreen.Functionality;
using MBOptionScreen.Settings;
using MBOptionScreen.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MBOptionScreen
{
    [Version("e1.0.0",  201)]
    [Version("e1.0.1",  201)]
    [Version("e1.0.2",  201)]
    [Version("e1.0.3",  201)]
    [Version("e1.0.4",  201)]
    [Version("e1.0.5",  201)]
    [Version("e1.0.6",  201)]
    [Version("e1.0.7",  201)]
    [Version("e1.0.8",  201)]
    [Version("e1.0.9",  201)]
    [Version("e1.0.10", 201)]
    [Version("e1.0.11", 201)]
    [Version("e1.1.0",  201)]
    [Version("e1.2.0",  201)]
    [Version("e1.2.1",  201)]
    [Version("e1.3.0",  201)]
    public sealed class DefaultMBOptionScreenInitializer : IMBOptionScreenInitializer
    {
        protected ApplicationVersion GameVerion { get; private set; }
        protected IApplicationContainerProvider ApplicationContainerProvider { get; private set; }
        private Legacy.v1.StateProviderV1 StateProviderV1 { get; } = new Legacy.v1.StateProviderV1();

        public void StartInitialization(ApplicationVersion gameVerion, bool first)
        {
            GameVerion = gameVerion;

            ApplicationContainerProvider = DI.GetImplementation<IApplicationContainerProvider, ApplicationContainerProviderWrapper>(GameVerion);
            if (first)
            {
                var settingsProvider = DI.GetImplementation<IMBOptionScreenSettingsProvider, SettingsProviderWrapper>(GameVerion);
                var modLibSettingsProvider = DI.GetImplementation<IModLibSettingsProvider, SettingsProviderWrapper>(GameVerion);
                ApplicationContainerProvider.Set("MBOptionScreenSettingsProvider", settingsProvider);
                ApplicationContainerProvider.Set("ModLibSettingsProvider", modLibSettingsProvider);

                Resolver.GameMenuScreenHandler.AddScreen(
                    "ModOptionsMenu_MBOptionScreen_v2",
                    9990,
                    () => (ScreenBase) DI.GetImplementation(GameVerion, typeof(MBOptionScreen).FullName),
                    new TextObject("{=HiZbHGvYG}Mod Options"));
            }

            // Settings providers are the only classes that need 'true' global access between MBOptionMods
            // get the global settings provider
            SettingsDatabase.MBOptionScreenSettingsProvider = new SettingsProviderWrapper(ApplicationContainerProvider.Get("MBOptionScreenSettingsProvider"));
            SettingsDatabase.ModLibSettingsProvider = new SettingsProviderWrapper(ApplicationContainerProvider.Get("ModLibSettingsProvider"));

            if (first)
            {
                PreventV1Loading();

                RegisterSettings();
            }
        }
        public void EndInitialization(bool first)
        {
            if (first)
            {
                Resolver.IngameMenuScreenHandler.AddScreen(
                    1,
                    () => (ScreenBase) DI.GetImplementation(GameVerion, typeof(MBOptionScreen).FullName),
                    new TextObject("{=NqarFr4P}Mod Options", null));

                if (MBOptionScreenSettings.Instance!.OverrideModLib)
                    Resolver.ModLibScreenOverrider.OverrideModLibScreen();


                ApplicationContainerProvider.Clear();
                StateProviderV1?.Clear();
            }
        }

        /// <summary>
        /// Find all implementations of SettingBase and register them
        /// </summary>
        private void RegisterSettings()
        {
            var settings = new List<SettingsBase>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Where(a => !a.FullName.StartsWith("System"))
                .Where(a => !a.FullName.StartsWith("Microsoft"))
                .Where(a => !a.FullName.StartsWith("mscorlib"))
                // ignore v1 classes
                .Where(a => Path.GetFileName(a.Location) != "MBOptionScreen.dll")
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            // ModLib
            var modLibSettings = allTypes
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, "ModLib.SettingsBase"))
                .Select(obj => new ModLibSettingsWrapper(Activator.CreateInstance(obj)));
            settings.AddRange(modLibSettings);
            if (settings.Count > 0)
                ModLibStub.LoadIfNeeded();

            var mbOptionScreenSettings = allTypes
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MBOptionScreen.Settings.SettingsBase"))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(StubSettings)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(SettingsWrapper)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(ModLibSettingsWrapper)))
#if !DEBUG
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TestSettingsBase<>)))
#endif
                ;
            var thisSettings = mbOptionScreenSettings
                .Where(t => ReflectionUtils.Implements(t, typeof(SettingsBase)))
                .Select(obj => (SettingsBase) Activator.CreateInstance(obj));
            settings.AddRange(thisSettings);
            var externalSettings = mbOptionScreenSettings
                .Where(t => !ReflectionUtils.Implements(t, typeof(SettingsBase)))
                .Select(obj => new SettingsWrapper(Activator.CreateInstance(obj)));
            settings.AddRange(externalSettings);

            foreach (var setting in settings)
                SettingsDatabase.RegisterSettings(setting);
        }


        private void PreventV1Loading()
        {
            var sharedStateObject = StateProviderV1.Get("MBOptionScreen.State.SharedStateObject");

            // Was not loaded
            // Replace SettingsProvider with our own implementation
            if (sharedStateObject == null)
            {
                StateProviderV1.CreateNewSharedState(SettingsDatabase.MBOptionScreenSettingsProvider);
            }
            // Was already loaded
            // Set SettingsProvider with our own implementation
            // Set HasInitialized to true to prevent EndInitialization()
            else
            {
                sharedStateObject.SetSettings(SettingsDatabase.MBOptionScreenSettingsProvider);
                sharedStateObject.SetInitialized();
                Resolver.GameMenuScreenHandler.RemoveScreen("ModOptionsMenu_MBOptionScreen");
            }

            // Seems that v1 SettingsDatabase.SettingsProvider is not overriden with v2 wrapper and
            // I can't even set it via reflection.
            // Not that big of a problem in theory. Harmony should still do the work.
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .FirstOrDefault(a => Path.GetFileName(a.Location) == "MBOptionScreen.dll");
            if (assembly != null)
                Legacy.v1.BaseSettingsDatabasePatch.PatchAll();
        }
    }
}