using HarmonyLib;

using System.Collections;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.ModLib.Functionality
{
    internal sealed class DefaultModLibScreenOverrider : BaseModLibScreenOverrider
    {
        private static readonly AccessTools.FieldRef<Module, IList> _initialStateOptions =
            AccessTools.FieldRefAccess<Module, IList>("_initialStateOptions");

        public override void OverrideModLibScreen()
        {
            var oldOptionScreen = Module.CurrentModule.GetInitialStateOptionWithId("ModOptionsMenu");
            if (oldOptionScreen != null)
            {
                _initialStateOptions(Module.CurrentModule).Remove(oldOptionScreen);
            }
        }
    }
}