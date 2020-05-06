using HarmonyLib;

using MCM.Abstractions.ApplicationContainer;
using MCM.Abstractions.Functionality;
using MCM.Abstractions.Synchronization;
using MCM.Implementation.ModLib.Settings.SettingsContainer;
using MCM.Utils;

using System;
using System.IO;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.ModLib
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        private ApplicationVersion GameVersion { get; }
        private IApplicationContainerProvider ApplicationContainerProvider { get; set; }

        public SubModuleV300()
        {
            GameVersion = ApplicationVersionUtils.GameVersion();
        }

        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnSubModuleLoad_ModLibv3" })!;
            if (synchronizationProvider.IsFirstInitialization)
            {
                ApplicationContainerProvider = DI.GetImplementation<IApplicationContainerProvider, ApplicationContainerProviderWrapper>(GameVersion)!;

                var modLibSettingsProvider = DI.GetImplementation<IModLibSettingsContainer, ModLibSettingsContainerWrapper>(GameVersion)!;
                ApplicationContainerProvider.Set("ModLibSettingsProvider", modLibSettingsProvider);

                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"));
                foreach (var assembly in assemblies)
                {
                    var settingsProviderWrapperType = assembly.GetType("MBOptionScreen.Settings.SettingsProviderWrapper");
                    var settingsDatabaseType = assembly.GetType("MBOptionScreen.Settings.SettingsDatabase");
                    var modLibSettingsProviderProperty = AccessTools.Property(settingsDatabaseType, "ModLibSettingsProvider");
                    modLibSettingsProviderProperty?.SetValue(
                        null,
                        Activator.CreateInstance(settingsProviderWrapperType, new object[] { modLibSettingsProvider }));
                }
            }
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnBeforeInitialModuleScreenSetAsRoot_ModLibv3" })!;
            if (synchronizationProvider.IsFirstInitialization)
            {
                if (MCMSettings.Instance!.OverrideModLib)
                    Resolver.ModLibScreenOverrider.OverrideModLibScreen();
            }
        }
    }
}