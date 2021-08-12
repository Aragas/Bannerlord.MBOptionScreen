using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib.BUTR.Extensions;

using System.Runtime.Serialization;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Utils
{
    internal static class HintViewModelUtils
    {
        private delegate HintViewModel EmptyDelegate();
        private delegate HintViewModel V1Delegate(string hintText, string? uniqueName = null);
        private delegate HintViewModel V2Delegate(TextObject? hintText, string? uniqueName = null);

        private static readonly EmptyDelegate? Empty;
        private static readonly V1Delegate? V1;
        private static readonly V2Delegate? V2;

        static HintViewModelUtils()
        {
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(typeof(HintViewModel), false))
            {
                var @params = constructorInfo.GetParameters();
                switch (@params.Length)
                {
                    case 0:
                        Empty = AccessTools2.GetDelegate<EmptyDelegate>(constructorInfo);
                        break;
                    case 2 when @params[0].ParameterType == typeof(string) && @params[1].ParameterType == typeof(string):
                        V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                        break;
                    case 2 when @params[0].ParameterType == typeof(TextObject) && @params[1].ParameterType == typeof(string):
                        V2 = AccessTools2.GetDelegate<V2Delegate>(constructorInfo);
                        break;
                }
            }
        }

        public static HintViewModel Create()
        {
            if (Empty is not null)
                return Empty();
            return (HintViewModel) FormatterServices.GetUninitializedObject(typeof(HintViewModel));
        }

        public static HintViewModel? Create(string text)
        {
            if (V1 is not null)
                return V1(text);
            if (V2 is not null)
                return V2(TextObjectHelper.Create(text));
            return null;
        }

        public static HintViewModel? Create(TextObject text)
        {
            if (V1 is not null)
                return V1(text.ToString());
            if (V2 is not null)
                return V2(text);
            return null;
        }
    }
}