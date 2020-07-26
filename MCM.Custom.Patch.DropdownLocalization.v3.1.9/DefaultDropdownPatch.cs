using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Custom.Patch.v319
{
    public static class DefaultDropdownPatch
    {
        private static readonly MethodInfo RefreshMethod =
            AccessTools.Method(typeof(SelectorVM<SelectorItemVM>), nameof(SelectorVM<SelectorItemVM>.Refresh), new []
            {
                typeof(IEnumerable<string>),
                typeof(int),
                typeof(Action<SelectorVM<SelectorItemVM>>)
            });

        public static bool Prefix(object dropdown, ref SelectorVM<SelectorItemVM> __result)
        {
            var type = dropdown.GetType();
            var selectorField = AccessTools.Field(type, "_selector");
            var selectedIndexField = AccessTools.Field(type, "_selectedIndex");
            var onSelectionChangedMethod = AccessTools.Method(type, "OnSelectionChanged");
            __result = (SelectorVM<SelectorItemVM>) selectorField.GetValue(dropdown);

            RefreshMethod.Invoke(__result, new []
            {
                (from object element in ((IList) dropdown) select new TextObject(element?.ToString() ?? "ERROR").ToString()),
                selectedIndexField.GetValue(dropdown),
                onSelectionChangedMethod.CreateDelegate(onSelectionChangedMethod.ReturnType, dropdown)
            });

            return true;
        }
    }
}
