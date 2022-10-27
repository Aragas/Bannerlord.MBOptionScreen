using HarmonyLib.BUTR.Extensions;

using System.Linq;
using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace MCM.UI.Utils
{
    public static class InitialMenuOptionVMUtils
    {
        private delegate object CtorV1Delegate(InitialStateOption initialStateOption);
        private static readonly CtorV1Delegate? CtorV1;

        static InitialMenuOptionVMUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenuOptionVM") ??
                       AccessTools2.TypeByName("TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu.InitialMenuOptionVM");
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(type) ?? Enumerable.Empty<ConstructorInfo>())
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 1 && @params[0].ParameterType == typeof(InitialStateOption))
                {
                    CtorV1 = AccessTools2.GetDelegate<CtorV1Delegate>(constructorInfo);
                }
            }
        }

        public static object? Create(InitialStateOption initialState)
        {
            if (CtorV1 is not null)
            {
                return CtorV1(initialState);
            }

            return null;
        }
    }
}