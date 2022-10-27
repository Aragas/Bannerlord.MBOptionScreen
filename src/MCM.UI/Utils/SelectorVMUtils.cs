using HarmonyLib.BUTR.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.UI.Utils
{
    public static class SelectorVMUtils
    {
        private delegate object CtorV1Delegate(IEnumerable<string> hintText, int selectedIndex, object? onChange);
        private static readonly CtorV1Delegate? CtorV1;

        static SelectorVMUtils()
        {
            var type1 = AccessTools2.TypeByName("TaleWorlds.Core.ViewModelCollection.SelectorVM`1") ??
                        AccessTools2.TypeByName("TaleWorlds.Core.ViewModelCollection.Selector.SelectorVM`1");

            var type2 = AccessTools2.TypeByName("TaleWorlds.Core.ViewModelCollection.SelectorItemVM") ??
                        AccessTools2.TypeByName("TaleWorlds.Core.ViewModelCollection.Selector.SelectorItemVM");

            var type = type1!.MakeGenericType(type2);

            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(type) ?? Enumerable.Empty<ConstructorInfo>())
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 3 && @params[0].ParameterType == typeof(IEnumerable<string>) && @params[1].ParameterType == typeof(int) && @params[2].ParameterType.IsGenericType)
                {
                    CtorV1 = AccessTools2.GetDelegate<CtorV1Delegate>(constructorInfo);
                }
            }
        }

        public static object? Create(IEnumerable<string> hintText, int selectedIndex, object? onChange)
        {
            if (CtorV1 is not null)
            {
                return CtorV1(hintText, selectedIndex, onChange);
            }

            return null;
        }
    }
}