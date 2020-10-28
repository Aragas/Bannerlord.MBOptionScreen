using HarmonyLib;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.MBO
{
    // Do not provide assembly substitutes
    public sealed class MCMImplementationMBOSubModule : MBSubModuleBase
    {
        /// <summary>
        /// Start initialization
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_MBOv3");
            if (synchronizationProvider.IsFirstInitialization)
            {
                var harmonyV1 = new Harmony("bannerlord.mcm.mbo.v1.loaderpreventer");
                foreach (var method in SettingsDatabaseV1Patch1.TargetMethods())
                {
                    harmonyV1.Patch(
                        method,
                        prefix: new HarmonyMethod(typeof(SettingsDatabaseV1Patch1), nameof(SettingsDatabaseV1Patch1.Prefix)));
                }
                /*
                foreach (var method in MBOptionScreenV1SubModulePatch1.TargetMethods())
                {
                    harmonyV1.Patch(
                        method,
                        prefix: new HarmonyMethod(typeof(MBOptionScreenV1SubModulePatch1), nameof(MBOptionScreenV1SubModulePatch1.Prefix)));
                }
                foreach (var method in MBOptionScreenV1SubModulePatch2.TargetMethods())
                {
                    harmonyV1.Patch(
                        method,
                        prefix: new HarmonyMethod(typeof(MBOptionScreenV1SubModulePatch2), nameof(MBOptionScreenV1SubModulePatch2.Prefix)));
                }
                */

                var harmonyV2 = new Harmony("bannerlord.mcm.mbo.v2.loaderpreventer");
                foreach (var method in SettingsDatabaseV2Patch1.TargetMethods())
                {
                    harmonyV2.Patch(
                        method,
                        prefix: new HarmonyMethod(typeof(SettingsDatabaseV2Patch1), nameof(SettingsDatabaseV2Patch1.Prefix)));
                }
                /*
                foreach (var method in MBOptionScreenV2SubModulePatch1.TargetMethods())
                {
                    harmonyV2.Patch(
                        method,
                        prefix: new HarmonyMethod(typeof(MBOptionScreenV2SubModulePatch1), nameof(MBOptionScreenV2SubModulePatch1.Prefix)));
                }
                foreach (var method in MBOptionScreenV2SubModulePatch2.TargetMethods())
                {
                    harmonyV2.Patch(
                        method,
                        prefix: new HarmonyMethod(typeof(MBOptionScreenV2SubModulePatch2), nameof(MBOptionScreenV2SubModulePatch2.Prefix)));
                }
                */
            }
        }

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnBeforeInitialModuleScreenSetAsRoot_MBOv3");
            if (synchronizationProvider.IsFirstInitialization) { }
        }
    }
}