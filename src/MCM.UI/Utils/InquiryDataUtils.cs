using HarmonyLib.BUTR.Extensions;

using System;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.UI.Utils
{
    internal static class InquiryDataUtils
    {
        private delegate InquiryData V1Delegate(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText,
            Action affirmativeAction, Action negativeAction, string soundEventPath = "", float expireTime = 0f, Action? timeoutAction = null);
        private static readonly V1Delegate? V1 =
            AccessTools2.GetConstructorDelegate<V1Delegate>(typeof(InquiryData), typeof(V1Delegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray());

        private delegate InquiryData V2Delegate(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText,
            Action affirmativeAction, Action negativeAction, string soundEventPath = "", float expireTime = 0f, Action? timeoutAction = null,
            Func<ValueTuple<bool, string>>? isAffirmativeOptionEnabled = null, Func<ValueTuple<bool, string>>? isNegativeOptionEnabled = null);
        private static readonly V2Delegate? V2 =
            AccessTools2.GetConstructorDelegate<V2Delegate>(typeof(InquiryData), typeof(V2Delegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray());

        public static InquiryData? Create(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText,
            Action affirmativeAction, Action negativeAction, string soundEventPath = "", float expireTime = 0f, Action? timeoutAction = null,
            Func<ValueTuple<bool, string>>? isAffirmativeOptionEnabled = null, Func<ValueTuple<bool, string>>? isNegativeOptionEnabled = null)
        {
            if (V1 is not null)
            {
                return V1(titleText, text, isAffirmativeOptionShown, isNegativeOptionShown, affirmativeText, negativeText, affirmativeAction, negativeAction, soundEventPath);
            }

            if (V2 is not null)
            {
                return V2(titleText, text, isAffirmativeOptionShown, isNegativeOptionShown, affirmativeText, negativeText, affirmativeAction, negativeAction, soundEventPath,
                    expireTime, timeoutAction, isAffirmativeOptionEnabled, isNegativeOptionEnabled);
            }

            return null;
        }
    }
}