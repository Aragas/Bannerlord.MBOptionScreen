using HarmonyLib;

using MCM.Abstractions.Synchronization;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.MBO
{
    // Do not provide assembly substitutes
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_MBOv3");
            if (synchronizationProvider.IsFirstInitialization)
            {
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
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnBeforeInitialModuleScreenSetAsRoot_MBOv3");
            if (synchronizationProvider.IsFirstInitialization)
            {

            }
        }
    }
}