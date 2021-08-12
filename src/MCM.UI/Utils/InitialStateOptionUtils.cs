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
        private delegate InitialStateOption V3Delegate(string id, TextObject name, int orderIndex, Action action, Func<ValueTuple<bool, TextObject?>> isDisabledAndReason);

        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;
        private static readonly V3Delegate? V3;

        static InitialStateOptionUtils()
        {
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(typeof(InitialStateOption), false))
            {
                var @params = constructorInfo.GetParameters();
                switch (@params.Length)
                {
                    case 5 when @params[4].ParameterType == typeof(bool):
                        V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                        break;
                    case 6 when @params[4].ParameterType == typeof(Func<bool>):
                        V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
                        break;
                    case 5 when @params[4].ParameterType == typeof(Func<ValueTuple<bool, TextObject>>):
                        V3 = AccessTools2.GetDelegate<V3Delegate>(constructorInfo);
                        break;
                }
            }
        }

        public static InitialStateOption? Create(string id, TextObject name, int orderIndex, Action action, Func<bool> isDisabled)
        {
            if (V1 is not null)
                return V1(id, name, orderIndex, action, isDisabled());
            if (V2 is not null)
                return V2(id, name, orderIndex, action, isDisabled);
            if (V3 is not null)
                return V3(id, name, orderIndex, action, () => (isDisabled(), null));
            return null;
        }
    }
}