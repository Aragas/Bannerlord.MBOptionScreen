using HarmonyLib.BUTR.Extensions;

using System;
using System.Linq;
using System.Reflection;

using TaleWorlds.Localization;

namespace MCM.Utils
{
    public static class EscapeMenuItemVMUtils
    {
        private delegate object CtorV1Delegate(TextObject item, Action<object> onExecute, object identifier, Func<Tuple<bool, TextObject>> getIsDisabledAndReason, bool isPositiveBehaviored = false);
        private static readonly CtorV1Delegate? CtorV1;

        static EscapeMenuItemVMUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenuItemVM") ??
                       AccessTools2.TypeByName("TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu.EscapeMenuItemVM");
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(type) ?? Enumerable.Empty<ConstructorInfo>())
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 5)
                {
                    CtorV1 = AccessTools2.GetDelegate<CtorV1Delegate>(constructorInfo);
                }
            }
        }

        public static object? Create(TextObject item, Action<object> onExecute, object identifier, Func<Tuple<bool, TextObject>> getIsDisabledAndReason, bool isPositiveBehaviored = false)
        {
            if (CtorV1 is not null)
            {
                return CtorV1(item, onExecute, identifier, getIsDisabledAndReason, isPositiveBehaviored);
            }

            return null;
        }
    }
}