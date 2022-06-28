using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Linq;
using System.Reflection;

namespace MCMv5.Tests.Utils
{
    public static class InquiryDataUtils
    {
        private delegate object CtorV1Delegate(string titleText,
            string text,
            bool isAffirmativeOptionShown,
            bool isNegativeOptionShown,
            string affirmativeText,
            string negativeText,
            Action affirmativeAction,
            Action negativeAction,
            string soundEventPath = "");
        private static readonly CtorV1Delegate? V1;
        
        private delegate object CtorV2Delegate(string titleText,
            string text,
            bool isAffirmativeOptionShown,
            bool isNegativeOptionShown,
            string affirmativeText,
            string negativeText,
            Action affirmativeAction,
            Action negativeAction,
            string soundEventPath = "",
            float expireTime = 0f,
            Action? timeoutAction = null);
        private static readonly CtorV2Delegate? V2;

        static InquiryDataUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.Library.InquiryData");
            foreach (var constructorInfo in AccessTools.GetDeclaredConstructors(type, false) ?? Enumerable.Empty<ConstructorInfo>())
            {
                var @params = constructorInfo.GetParameters();
                if (@params.Length == 9)
                {
                    V1 = AccessTools2.GetDelegate<CtorV1Delegate>(constructorInfo);
                }
                if (@params.Length == 11)
                {
                    V2 = AccessTools2.GetDelegate<CtorV2Delegate>(constructorInfo);
                }
            }
        }

        public static InquiryDataWrapper? Create(string titleText,
            string text,
            bool isAffirmativeOptionShown,
            bool isNegativeOptionShown,
            string affirmativeText,
            string negativeText,
            Action affirmativeAction,
            Action negativeAction,
            string soundEventPath = "",
            float expireTime = 0f,
            Action? timeoutAction = null)
        {
            if (V1 is not null)
            {
                var obj = V1(titleText,
                    text,
                    isAffirmativeOptionShown,
                    isNegativeOptionShown,
                    affirmativeText,
                    negativeText,
                    affirmativeAction,
                    negativeAction,
                    soundEventPath);
                return InquiryDataWrapper.Create(obj);
            }
            
            if (V2 is not null)
            {
                var obj = V2(titleText,
                    text,
                    isAffirmativeOptionShown,
                    isNegativeOptionShown,
                    affirmativeText,
                    negativeText,
                    affirmativeAction,
                    negativeAction,
                    soundEventPath,
                    expireTime,
                    timeoutAction);
                return InquiryDataWrapper.Create(obj);
            }

            return null;
        }
    }
}