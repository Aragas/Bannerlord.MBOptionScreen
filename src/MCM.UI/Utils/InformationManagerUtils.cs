using HarmonyLib.BUTR.Extensions;

using System;

namespace MCM.UI.Utils
{
    internal static class InformationManagerUtils
    {
        private delegate void ShowInquiryV1Delegate(object data, bool pauseGameActiveState = false);

        private static readonly ShowInquiryV1Delegate? ShowInquiryV1;

        static InformationManagerUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.Core.InformationManager");
            foreach (var methodInfo in HarmonyLib.AccessTools.GetDeclaredMethods(type))
            {
                var @params = methodInfo.GetParameters();
                if (@params.Length == 2 && @params[0].ParameterType.Name.Equals("InquiryData", StringComparison.Ordinal) && @params[1].ParameterType == typeof(bool))
                {
                    ShowInquiryV1 = AccessTools2.GetDelegate<ShowInquiryV1Delegate>(methodInfo);
                }
            }
        }

        public static void ShowInquiry(InquiryDataWrapper? data, bool pauseGameActiveState = false)
        {
            if (data is null)
                return;

            if (ShowInquiryV1 is not null)
            {
                ShowInquiryV1(data.Object, pauseGameActiveState);
            }
        }
    }
}