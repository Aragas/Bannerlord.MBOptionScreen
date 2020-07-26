using System;
using System.Linq;
using HarmonyLib;

using MCM.Utils;

using TaleWorlds.MountAndBlade;

namespace MCM.Custom.Patch.v319
{
    public sealed class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            var harmony = new Harmony("bannerlord.mcm.custom.patch.presetlocalization.v319");

            var settingsVMType = Type.GetType("MCM.UI.GUI.ViewModels.SettingsVM, MCMv3.UI.v3.1.9");
            if (settingsVMType != null)
            {
                harmony.Patch(
                    settingsVMType.GetConstructors().First(),
                    postfix: new HarmonyMethod(typeof(SettingsVMPatch), nameof(SettingsVMPatch.Postfix)));
            }
        }
    }
}