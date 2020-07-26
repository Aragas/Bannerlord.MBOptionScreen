using HarmonyLib;

using MCM.Abstractions.Ref;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.Custom.Patch.v319
{
    public static class DefaultDropdownPatch
    {
        private static readonly PropertyInfo PropertyReferenceProperty =
            AccessTools.Property(Type.GetType("MCM.UI.GUI.ViewModels.SettingsPropertyVM, MCMv3.UI.v3.1.9"), "PropertyReference");

        private static readonly FieldInfo ItemListField = AccessTools.Field(typeof(SelectorVM<SelectorItemVM>), "_itemList");
        private static readonly FieldInfo ListField = AccessTools.Field(typeof(MBBindingList<SelectorItemVM>), "_list");

        public static void PostfixVM(object __instance, ref SelectorVM<SelectorItemVM> __result)
        {
            var dropdown = ((IRef) PropertyReferenceProperty.GetValue(__instance)).Value;
            if (dropdown is IList dropdownList)
            {
                var selector = __result;

                var list = (List<SelectorItemVM>) ListField.GetValue(ItemListField.GetValue(selector));
                list.Clear();
                list.AddRange(from str in from object element in dropdownList select new TextObject(element?.ToString() ?? "ERROR").ToString() select new SelectorItemVM(str));
            }
        }

        public static void PostfixConstructor(object __instance, IEnumerable<object> values, int selectedIndex)
        {
            var selectorField = AccessTools.Field(__instance.GetType(), "_selector");
            var onSelectionChangedMethod = AccessTools.Method(__instance.GetType(), "OnSelectionChanged");

            var selector = new SelectorVM<SelectorItemVM>(
                values.Select(x => x?.ToString()?? "ERROR"),
                selectedIndex,
                (Action<SelectorVM<SelectorItemVM>>) Delegate.CreateDelegate(typeof(Action<SelectorVM<SelectorItemVM>>), __instance, onSelectionChangedMethod));
            selectorField.SetValue(__instance, selector);
        }
    }
}