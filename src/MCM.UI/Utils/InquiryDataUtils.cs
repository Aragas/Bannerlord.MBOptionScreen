using HarmonyLib.BUTR.Extensions;

using System;

namespace MCM.UI.Utils
{
    internal sealed class InquiryDataWrapper
    {
        public static InquiryDataWrapper Create(object @object) => new(@object);

        public object Object { get; }

        private InquiryDataWrapper(object @object)
        {
            Object = @object;
        }
    }

    /// <summary>
    /// Introduced V1, V2 by e1.7.0
    /// </summary>
    internal static class InquiryDataUtils
    {
        private delegate object V1Delegate(
            string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText,
            Action affirmativeAction, Action negativeAction, string soundEventPath = "");

        private static readonly V1Delegate? V1;

        static InquiryDataUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.Core.InquiryData") ?? AccessTools2.TypeByName("TaleWorlds.Library.InquiryData");
            foreach (var constructorInfo in HarmonyLib.AccessTools.GetDeclaredConstructors(type, false))
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 9)
                {
                    V1 = AccessTools2.GetDelegate<V1Delegate>(constructorInfo);
                }
            }
        }

        public static InquiryDataWrapper? Create(
            string? titleText, string? text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string? affirmativeText, string? negativeText,
            Action affirmativeAction, Action negativeAction, string soundEventPath = "")
        {
            if (V1 is not null)
            {
                var obj = V1(titleText, text, isAffirmativeOptionShown, isNegativeOptionShown, affirmativeText, negativeText, affirmativeAction, negativeAction, soundEventPath);
                return InquiryDataWrapper.Create(obj);
            }

            return null;
        }
    }
}