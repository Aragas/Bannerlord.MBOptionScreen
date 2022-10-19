using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Collections;

using TaleWorlds.MountAndBlade;

namespace MCM.Adapter.ModLib.Functionality
{
    internal sealed class DefaultModLibScreenOverrider : BaseModLibScreenOverrider
    {
        private static readonly AccessTools.FieldRef<Module, IList>? InitialStateOptions =
            AccessTools2.FieldRefAccess<Module, IList>("_initialStateOptions");

        public override void OverrideModLibScreen()
        {
            var oldOptionScreen = Module.CurrentModule.GetInitialStateOptionWithId("ModOptionsMenu");
            if (oldOptionScreen is not null)
            {
                InitialStateOptions?.Invoke(Module.CurrentModule).Remove(oldOptionScreen);
            }
        }
    }
}