using Bannerlord.UIExtenderEx;

using HarmonyLib;

using MCM.Abstractions.Synchronization;
using MCM.UI.Patches;

using System.Runtime.CompilerServices;

using TaleWorlds.MountAndBlade;

[assembly: InternalsVisibleTo("MCM.Custom.ScreenTests")]
[assembly: InternalsVisibleTo("MCM.Tests")]

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
                var loadingHarmony = new Harmony("bannerlord.mcm.ui.loading");
                loadingHarmony.Patch(
                    MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadTargetMethod,
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnGauntletUISubModuleSubModuleLoadPostfix)));
                loadingHarmony.Patch(
                    MBSubModuleBasePatch.OnSubModuleUnloadedTargetMethod,
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnSubModuleUnloadedPostfix)));
                loadingHarmony.Patch(
                    MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootTargetMethod,
                    postfix: new HarmonyMethod(typeof(MBSubModuleBasePatch), nameof(MBSubModuleBasePatch.OnBeforeInitialModuleScreenSetAsRootPostfix)));

                var editabletextpatchHarmony = new Harmony("bannerlord.mcm.ui.editabletextpatch");
                editabletextpatchHarmony.Patch(
                    EditableTextPatch.GetCursorPositionMethod,
                    finalizer: new HarmonyMethod(typeof(EditableTextPatch), nameof(EditableTextPatch.GetCursorPosition)));

                var viewmodelwrapperHarmony = new Harmony("bannerlord.mcm.ui.viewmodelwrapper");
                viewmodelwrapperHarmony.Patch(
                    ViewModelPatch.ExecuteCommandMethod,
                    prefix: new HarmonyMethod(typeof(ViewModelPatch), nameof(ViewModelPatch.ExecuteCommandPatch)));
            }
        }
    }
}