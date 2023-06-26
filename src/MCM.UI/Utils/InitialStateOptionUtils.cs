using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MCM.UI.Utils
{
    internal static class InitialStateOptionUtils
    {
        private delegate InitialStateOption V1Delegate(string id, TextObject name, int orderIndex, Action action, Func<(bool, TextObject?)> isDisabledAndReason);
        private delegate InitialStateOption V2Delegate(string id, TextObject name, int orderIndex, Action action, Func<(bool, TextObject?)> isDisabledAndReason, TextObject? enabledHint = null);

        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;

        static InitialStateOptionUtils()
        {
            foreach (var constructorInfo in AccessTools.GetDeclaredConstructors(typeof(InitialStateOption), false))
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 5)
                    V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                if (@params.Length == 6)
                    V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
            }
        }

        public static InitialStateOption Create(string id, TextObject name, int orderIndex, Action action, Func<(bool, TextObject?)> isDisabledAndReason)
        {
            if (V1 is not null)
                return V1(id, name, orderIndex, action, isDisabledAndReason);
            if (V2 is not null)
                return V2(id, name, orderIndex, action, isDisabledAndReason, null);
            return null;
        }
    }
}