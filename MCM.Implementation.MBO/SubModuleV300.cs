using HarmonyLib;

using MCM.Abstractions.ApplicationContainer;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Abstractions.Synchronization;
using MCM.Utils;

using System;
using System.IO;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.MBO
{
    // Do not provide assembly substitutes
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
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnSubModuleLoad_MBOv3" })!;
            if (synchronizationProvider.IsFirstInitialization)
            {
                ApplicationContainerProvider = DI.GetImplementation<IApplicationContainerProvider, ApplicationContainerProviderWrapper>(GameVersion)!;

                var settingsProvider = DI.GetImplementation<IGlobalSettingsContainer, SettingsContainerWrapper>(GameVersion)!;
                ApplicationContainerProvider.Set("MBOptionScreenSettingsProvider", settingsProvider);

                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Where(a => Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"));
                foreach (var assembly in assemblies)
                {
                    var settingsProviderWrapperType = assembly.GetType("MBOptionScreen.Settings.SettingsProviderWrapper");
                    var settingsDatabaseType = assembly.GetType("MBOptionScreen.Settings.SettingsDatabase");
                    var mbOptionScreenSettingsProviderProperty = AccessTools.Property(settingsDatabaseType, "MBOptionScreenSettingsProvider");
                    mbOptionScreenSettingsProviderProperty?.SetValue(
                        null,
                        Activator.CreateInstance(settingsProviderWrapperType, new object[] { settingsProvider }));
                }


                var harmonyV1 = new Harmony("bannerlord.mcm.v1.loaderpreventer");
                foreach (var method in v1.SettingsDatabasePatch1.TargetMethods())
                {
                    harmonyV1.Patch(
                        original: method,
                        prefix: new HarmonyMethod(typeof(v1.SettingsDatabasePatch1), nameof(v1.SettingsDatabasePatch1.Prefix)));
                }
                foreach (var method in v1.MBOptionScreenSubModulePatch1.TargetMethods())
                {
                    harmonyV1.Patch(
                        original: method,
                        prefix: new HarmonyMethod(typeof(v1.MBOptionScreenSubModulePatch1), nameof(v1.MBOptionScreenSubModulePatch1.Prefix)));
                }
                foreach (var method in v1.MBOptionScreenSubModulePatch2.TargetMethods())
                {
                    harmonyV1.Patch(
                        original: method,
                        prefix: new HarmonyMethod(typeof(v1.MBOptionScreenSubModulePatch2), nameof(v1.MBOptionScreenSubModulePatch2.Prefix)));
                }

                var harmonyV2 = new Harmony("bannerlord.mcm.v2.loaderpreventer");
                foreach (var method in v2.SettingsDatabasePatch1.TargetMethods())
                {
                    harmonyV2.Patch(
                        original: method,
                        prefix: new HarmonyMethod(typeof(v2.SettingsDatabasePatch1), nameof(v2.SettingsDatabasePatch1.Prefix)));
                }
                foreach (var method in v2.MBOptionScreenSubModulePatch1.TargetMethods())
                {
                    harmonyV2.Patch(
                        original: method,
                        prefix: new HarmonyMethod(typeof(v2.MBOptionScreenSubModulePatch1), nameof(v2.MBOptionScreenSubModulePatch1.Prefix)));
                }
                foreach (var method in v2.MBOptionScreenSubModulePatch2.TargetMethods())
                {
                    harmonyV2.Patch(
                        original: method,
                        prefix: new HarmonyMethod(typeof(v2.MBOptionScreenSubModulePatch2), nameof(v2.MBOptionScreenSubModulePatch2.Prefix)));
                }
            }
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = DI.GetImplementation<ISynchronizationProvider, SynchronizationProviderWrapper>(GameVersion, new object[] { "OnBeforeInitialModuleScreenSetAsRoot_MBOv3" })!;
            if (synchronizationProvider.IsFirstInitialization)
            {

            }
        }
    }
}