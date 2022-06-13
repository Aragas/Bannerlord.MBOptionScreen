using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Linq;
using System.Reflection;

using TaleWorlds.Library;

namespace MCM.Utils
{
    public static class InformationMessageUtils
    {
        private delegate object CtorV1Delegate(string information, Color color);
        private static readonly CtorV1Delegate? V1;

        static InformationMessageUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.Core.InformationMessage") ??
                       AccessTools2.TypeByName("TaleWorlds.Library.InformationMessage");
            foreach (var constructorInfo in AccessTools.GetDeclaredConstructors(type, false) ?? Enumerable.Empty<ConstructorInfo>())
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 9)
                {
                    V1 = AccessTools2.GetDelegate<CtorV1Delegate>(constructorInfo);
                }
            }
        }

        public static InformationMessageWrapper? Create(string information, Color color)
        {
            if (V1 is not null)
            {
                var obj = V1(information, color);
                return InformationMessageWrapper.Create(obj);
            }

            return null;
        }
    }
}