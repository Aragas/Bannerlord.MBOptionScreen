using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Utils
{
    internal static class HintViewModelUtils
    {
        private delegate HintViewModel EmptyDelegate();
        private delegate HintViewModel V1Delegate(string hintText, string uniqueName = null);
        private delegate HintViewModel V2Delegate(TextObject hintText, string uniqueName = null);

        private static readonly EmptyDelegate? Empty;
        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;

        static HintViewModelUtils()
        {
            foreach (var constructorInfo in AccessTools.GetDeclaredConstructors(typeof(HintViewModel), false))
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 0)
                    Empty = AccessTools2.GetDelegate<EmptyDelegate>(constructorInfo);
                if (@params.Length >= 1 && @params[1].ParameterType == typeof(string))
                    V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                if (@params.Length >= 1 && @params[1].ParameterType == typeof(TextObject))
                    V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
            }
        }

        public static HintViewModel Create()
        {
            if (Empty is not null)
                return Empty();
            return null;
        }

        public static HintViewModel Create(string text)
        {
            if (V1 is not null)
                return V1(text);
            if (V2 is not null)
                return V2(TextObjectHelper.Create(text));
            return null;
        }

        public static HintViewModel Create(TextObject text)
        {
            if (V1 is not null)
                return V1(text.ToString());
            if (V2 is not null)
                return V2(text);
            return null;
        }
    }
}