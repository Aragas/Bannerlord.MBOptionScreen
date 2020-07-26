using System;
using System.Linq;

using HarmonyLib;

using MCM.Abstractions.Data;

using TaleWorlds.MountAndBlade;

namespace MCM.Custom.Patch.v319
{
    public sealed class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            var harmony = new Harmony("bannerlord.mcm.custom.patch.dropdownlocalization.v319");

            harmony.Patch(
                typeof(DefaultDropdown<object>).GetConstructors().First(),
                postfix: new HarmonyMethod(typeof(DefaultDropdownPatch), nameof(DefaultDropdownPatch.PostfixConstructor)));

            var settingsPropertyVMType = Type.GetType("MCM.UI.GUI.ViewModels.SettingsPropertyVM, MCMv3.UI.v3.1.9");
            if (settingsPropertyVMType != null)
            {
                harmony.Patch(
                    AccessTools.PropertyGetter(settingsPropertyVMType, "DropdownValue"),
                    postfix: new HarmonyMethod(typeof(DefaultDropdownPatch), nameof(DefaultDropdownPatch.PostfixVM)));
            }
        }
    }
}