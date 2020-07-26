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
            var harmony = new Harmony("bannerlord.mcm.custom.patch.dropdownlocalization.v319");

            harmony.Patch(
                AccessTools.Method(typeof(SettingsUtils), nameof(SettingsUtils.GetSelector)),
                prefix: new HarmonyMethod(typeof(DefaultDropdownPatch), nameof(DefaultDropdownPatch.Prefix)));
        }
    }
}