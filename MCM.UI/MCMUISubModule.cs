using Bannerlord.UIExtenderEx;

using HarmonyLib;

using MCM.Abstractions.Synchronization;

using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class MCMUISubModule : MBSubModuleBase
    {
        public static readonly UIExtender _extender = new UIExtender("MCM.UI");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            using var synchronizationProvider = BaseSynchronizationProvider.Create("OnSubModuleLoad_UIv3");
            if (synchronizationProvider.IsFirstInitialization)
            {
                var harmony = new Harmony("bannerlord.mcm.ui.loading");
                harmony.Patch(
                    MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadTargetMethod,
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadPostfix)));
                harmony.Patch(
                    MBSubModuleBasePatch.OnSubModuleUnloadedTargetMethod,
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnSubModuleUnloadedPostfix)));
                harmony.Patch(
                    MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootTargetMethod,
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootPostfix)));
            }
        }
    }
}