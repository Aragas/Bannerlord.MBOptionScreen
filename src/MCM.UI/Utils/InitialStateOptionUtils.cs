using HarmonyLib.BUTR.Extensions;

using System;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MCM.UI.Utils
{
    internal static class InitialStateOptionUtils
    {
        private delegate InitialStateOption V1Delegate(string id, TextObject name, int orderIndex, Action action, bool isDisabled);
        private delegate InitialStateOption V2Delegate(string id, TextObject name, int orderIndex, Action action, Func<bool> isDisabled, TextObject? disabledReason = null);

        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;

        static InitialStateOptionUtils()
        {
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(typeof(InitialStateOption), false))
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length >= 5 && @params[4].ParameterType == typeof(bool))
                    V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                if (@params.Length >= 5 && @params[4].ParameterType == typeof(Func<bool>))
                    V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
            }
        }

        public static InitialStateOption? Create(string id, TextObject name, int orderIndex, Action action, Func<bool> isDisabled)
        {
            if (V1 is not null)
                return V1(id, name, orderIndex, action, isDisabled());
            if (V2 is not null)
                return V2(id, name, orderIndex, action, isDisabled);
            return null;
        }
    }
}