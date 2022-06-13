using HarmonyLib.BUTR.Extensions;

using System;
using System.Linq;
using System.Reflection;

namespace MCM.Utils
{
    public static class InformationManagerUtils
    {
        private delegate void DisplayMessageV1Delegate(object data);
        private static readonly DisplayMessageV1Delegate? DisplayMessageV1;
        
        private delegate void ShowInquiryV1Delegate(object data, bool pauseGameActiveState = false);
        private static readonly ShowInquiryV1Delegate? ShowInquiryV1;

        static InformationManagerUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.Core.InformationManager") ??
                       AccessTools2.TypeByName("TaleWorlds.Library.InformationManager");
            foreach (var methodInfo in HarmonyLib.AccessTools.GetDeclaredMethods(type)  ?? Enumerable.Empty<MethodInfo>())
            {
                var @params = methodInfo.GetParameters();
                if (@params.Length == 1 && @params[0].ParameterType.Name.Equals("InformationMessage", StringComparison.Ordinal))
                {
                    DisplayMessageV1 = AccessTools2.GetDelegate<DisplayMessageV1Delegate>(methodInfo);
                }
                if (@params.Length == 2 && @params[0].ParameterType.Name.Equals("InquiryData", StringComparison.Ordinal) && @params[1].ParameterType == typeof(bool))
                {
                    ShowInquiryV1 = AccessTools2.GetDelegate<ShowInquiryV1Delegate>(methodInfo);
                }
            }
        }

        public static void DisplayMessage(InformationMessageWrapper? message)
        {
            if (message is null)
                return;

            if (DisplayMessageV1 is not null)
            {
                DisplayMessageV1(message.Object);
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