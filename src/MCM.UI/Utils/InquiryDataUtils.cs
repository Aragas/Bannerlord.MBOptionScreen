using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

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


        private delegate TextInquiryData V1TextDelegate(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText,
            Action<string> affirmativeAction, Action negativeAction, bool shouldInputBeObfuscated = false, Func<string, Tuple<bool, string>>? textCondition = null, string soundEventPath = "",
            string defaultInputText = "");
        private static readonly V1TextDelegate? V1Text =
            AccessTools2.GetConstructorDelegate<V1TextDelegate>(typeof(TextInquiryData), typeof(V1TextDelegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray());


        private delegate MultiSelectionInquiryData V1MultiDelegate(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int maxSelectableOptionCount,
            string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction, string soundEventPath = "");
        private static readonly V1MultiDelegate? V1Multi =
            AccessTools2.GetConstructorDelegate<V1MultiDelegate>(typeof(MultiSelectionInquiryData), typeof(V1MultiDelegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray());

        private delegate MultiSelectionInquiryData V2MultiDelegate(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount,
            string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction, string soundEventPath = "");
        private static readonly V2MultiDelegate? V2Multi =
            AccessTools2.GetConstructorDelegate<V2MultiDelegate>(typeof(MultiSelectionInquiryData), typeof(V2MultiDelegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray());

        private delegate MultiSelectionInquiryData V3MultiDelegate(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount,
            string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction, string soundEventPath = "", bool isSeachAvailable = false);
        private static readonly V3MultiDelegate? V3Multi =
            AccessTools2.GetConstructorDelegate<V3MultiDelegate>(typeof(MultiSelectionInquiryData), typeof(V3MultiDelegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray());


        public static InquiryData? Create(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action affirmativeAction, Action negativeAction)
        {
            if (V1 is not null)
                return V1(titleText, text, isAffirmativeOptionShown, isNegativeOptionShown, affirmativeText, negativeText, affirmativeAction, negativeAction);

            if (V2 is not null)
                return V2(titleText, text, isAffirmativeOptionShown, isNegativeOptionShown, affirmativeText, negativeText, affirmativeAction, negativeAction);

            return null;
        }

        public static InquiryData? CreateTranslatable(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action affirmativeAction, Action negativeAction) =>
            Create(new TextObject(titleText).ToString(), new TextObject(text).ToString(), isAffirmativeOptionShown, isNegativeOptionShown, new TextObject(affirmativeText).ToString(), new TextObject(negativeText).ToString(), affirmativeAction, negativeAction);


        public static TextInquiryData? CreateText(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action<string> affirmativeAction, Action negativeAction)
        {
            if (V1Text is not null)
                return V1Text(titleText, text, isAffirmativeOptionShown, isNegativeOptionShown, affirmativeText, negativeText, affirmativeAction, negativeAction);

            return null;
        }

        public static TextInquiryData? CreateTextTranslatable(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action<string> affirmativeAction, Action negativeAction) =>
            CreateText(new TextObject(titleText).ToString(), new TextObject(text).ToString(), isAffirmativeOptionShown, isNegativeOptionShown, new TextObject(affirmativeText).ToString(), new TextObject(negativeText).ToString(), affirmativeAction, negativeAction);


        public static MultiSelectionInquiryData? CreateMulti(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount, string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction)
        {
            if (V1Multi is not null)
                return V1Multi(titleText, descriptionText, inquiryElements, isExitShown, maxSelectableOptionCount, affirmativeText, negativeText, affirmativeAction, negativeAction);

            if (V2Multi is not null)
                return V2Multi(titleText, descriptionText, inquiryElements, isExitShown, minSelectableOptionCount, maxSelectableOptionCount, affirmativeText, negativeText, affirmativeAction, negativeAction);

            if (V3Multi is not null)
                return V3Multi(titleText, descriptionText, inquiryElements, isExitShown, minSelectableOptionCount, maxSelectableOptionCount, affirmativeText, negativeText, affirmativeAction, negativeAction);

            return null;
        }

        public static MultiSelectionInquiryData? CreateMultiTranslatable(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount, string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction) =>
            CreateMulti(new TextObject(titleText).ToString(), new TextObject(descriptionText).ToString(), inquiryElements, isExitShown, minSelectableOptionCount, maxSelectableOptionCount, new TextObject(affirmativeText).ToString(), new TextObject(negativeText).ToString(), affirmativeAction, negativeAction);
    }
}