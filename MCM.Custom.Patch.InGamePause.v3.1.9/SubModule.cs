using HarmonyLib;

using System;

using TaleWorlds.MountAndBlade;

namespace MCM.Custom.Patch.v319
{
    public sealed class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            var harmony = new Harmony("bannerlord.mcm.custom.patch.ingamepause.v319");

            var defaultIngameMenuScreenHandlerType = Type.GetType("MCM.Implementation.Functionality.DefaultIngameMenuScreenHandler, MCMv3.Implementation.v3.1.9");
            if (defaultIngameMenuScreenHandlerType != null)
            {
                harmony.Patch(
                    AccessTools.Method(defaultIngameMenuScreenHandlerType.GetNestedType("<>c__DisplayClass6_1", AccessTools.all), "<MapScreen_GetEscapeMenuItems>b__0"),
                    transpiler: new HarmonyMethod(
                        typeof(DefaultIngameMenuScreenHandlerPatch),
                        nameof(DefaultIngameMenuScreenHandlerPatch.Transpiler)));

                harmony.Patch(
                    AccessTools.Method(defaultIngameMenuScreenHandlerType.GetNestedType("<>c__DisplayClass7_1", AccessTools.all), "<MissionSingleplayerEscapeMenu_GetEscapeMenuItems>b__0"),
                    transpiler: new HarmonyMethod(
                        typeof(DefaultIngameMenuScreenHandlerPatch),
                        nameof(DefaultIngameMenuScreenHandlerPatch.Transpiler)));
            }
        }
    }
}