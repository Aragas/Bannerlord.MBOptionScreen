using HarmonyLib;

using MCM.Abstractions.Synchronization;

using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_UIv3");
            if (synchronizationProvider.IsFirstInitialization)
            {
                var harmony = new Harmony("bannerlord.mcm.ui.loading_v3");
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadPostfix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnSubModuleUnloadedTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnSubModuleUnloadedPostfix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootPostfix)));
            }
        }
    }
}