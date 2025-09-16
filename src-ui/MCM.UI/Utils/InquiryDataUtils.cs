using System;
using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.Utils
{
    internal static class InquiryDataUtils
    {
        public static InquiryData CreateTranslatable(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action affirmativeAction, Action negativeAction) =>
            new(new TextObject(titleText).ToString(), new TextObject(text).ToString(), isAffirmativeOptionShown, isNegativeOptionShown, new TextObject(affirmativeText).ToString(), new TextObject(negativeText).ToString(), affirmativeAction, negativeAction);
        
        public static TextInquiryData CreateTextTranslatable(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action<string> affirmativeAction, Action negativeAction) =>
            new(new TextObject(titleText).ToString(), new TextObject(text).ToString(), isAffirmativeOptionShown, isNegativeOptionShown, new TextObject(affirmativeText).ToString(), new TextObject(negativeText).ToString(), affirmativeAction, negativeAction);


        public static MultiSelectionInquiryData? CreateMulti(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount, string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction)
        {
#if v100 || v101 || v102 || v103 || v110 || v111 || v112 || v113 || v114 || v115 || v116
            return new MultiSelectionInquiryData(
                titleText,
                descriptionText,
                inquiryElements,
                isExitShown,
                maxSelectableOptionCount,
                affirmativeText,
                negativeText,
                affirmativeAction,
                negativeAction);
#elif v124 || v125 || v126 || v127 || v128 || v129 || v1210 || v1211 || v1212 || v130
            return new MultiSelectionInquiryData(
                titleText,
                descriptionText,
                inquiryElements,
                isExitShown,
                minSelectableOptionCount,
                maxSelectableOptionCount,
                affirmativeText,
                negativeText,
                affirmativeAction,
                negativeAction);
            
#else
            return new MultiSelectionInquiryData(
                titleText,
                descriptionText,
                inquiryElements,
                isExitShown,
                maxSelectableOptionCount,
                affirmativeText,
                negativeText,
                affirmativeAction,
                negativeAction);
#endif
        }

        public static MultiSelectionInquiryData? CreateMultiTranslatable(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount, string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction) =>
            CreateMulti(new TextObject(titleText).ToString(), new TextObject(descriptionText).ToString(), inquiryElements, isExitShown, minSelectableOptionCount, maxSelectableOptionCount, new TextObject(affirmativeText).ToString(), new TextObject(negativeText).ToString(), affirmativeAction, negativeAction);
    }
}