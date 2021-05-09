using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Engine.GauntletUI;

namespace MCM.UI.Utils
{
    internal static class GauntletLayerUtils
    {
        private delegate GauntletLayer V1Delegate(int localOrder, string categoryId = "GauntletLayer");
        private delegate GauntletLayer V2Delegate(int localOrder, string categoryId = "GauntletLayer", bool shouldClear = false);

        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;

        static GauntletLayerUtils()
        {
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(typeof(GauntletLayer), false))
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 2)
                    V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                if (@params.Length == 3)
                    V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
            }
        }

        public static GauntletLayer? Create(int localOrder, string categoryId = "GauntletLayer", bool shouldClear = false)
        {
            if (V1 is not null)
                return V1(localOrder, categoryId);
            if (V2 is not null)
                return V2(localOrder, categoryId, shouldClear);
            return null;
        }
    }
}