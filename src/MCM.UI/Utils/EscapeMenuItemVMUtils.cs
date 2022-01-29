using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib.BUTR.Extensions;

using System;

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MCM.UI.Utils
{
    /// <summary>
    /// Introduced V3 by e1.7.1
    /// </summary>
    internal static class EscapeMenuItemVMUtils
    {
        private delegate EscapeMenuItemVM V1Delegate(TextObject item, Action<object> onExecute, object identifier, bool isDisabled, bool isPositiveBehavioured = false);
        private delegate EscapeMenuItemVM V2Delegate(TextObject item, Action<object> onExecute, object identifier, Tuple<bool, TextObject> isDisabledAndReason, bool isPositiveBehavioured = false);
        private delegate EscapeMenuItemVM V3Delegate(TextObject item, Action<object> onExecute, object identifier, Func<Tuple<bool, TextObject>> isDisabledAndReason, bool isPositiveBehavioured = false);

        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;
        private static readonly V3Delegate? V3;

        static EscapeMenuItemVMUtils()
        {
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(typeof(EscapeMenuItemVM), false))
            {
                var @params = constructorInfo.GetParameters();
                switch (@params.Length)
                {
                    case 5 when @params[4].ParameterType == typeof(bool):
                        V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                        break;
                    case 5 when @params[4].ParameterType == typeof(Tuple<bool, TextObject>):
                        V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
                        break;
                    case 5 when @params[4].ParameterType == typeof(Func<Tuple<bool, TextObject>>):
                        V3 = AccessTools2.GetDelegate<V3Delegate>(constructorInfo);
                        break;
                }
            }
        }

        public static EscapeMenuItemVM? Create(TextObject item, Action<object> onExecute, object identifier, bool isDisabled, bool isPositiveBehavioured = false)
        {
            if (V1 is not null)
                return V1(item, onExecute, identifier, isDisabled, isPositiveBehavioured);
            if (V2 is not null)
                return V2(item, onExecute, identifier, new Tuple<bool, TextObject>(isDisabled, TextObjectHelper.Create(string.Empty)!), isPositiveBehavioured);
            if (V3 is not null)
                return V3(item, onExecute, identifier, () => new Tuple<bool, TextObject>(isDisabled, TextObjectHelper.Create(string.Empty)!), isPositiveBehavioured);
            return null;
        }
    }
}