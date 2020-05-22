using HarmonyLib;

using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

                var harmony = new Harmony("bannerlord.mcm.ui.loading_v3");
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnSubModuleLoadTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnSubModuleLoadPostfix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnSubModuleUnloadedTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnSubModuleUnloadedPostfix)));
                harmony.Patch(
                    original: MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootTargetMethod(),
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootPostfix)));
        }
    }
}