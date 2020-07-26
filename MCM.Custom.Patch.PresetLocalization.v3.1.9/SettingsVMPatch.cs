using HarmonyLib;

using MCM.Abstractions.Settings.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace MCM.Custom.Patch.v319
{
    public static class SettingsVMPatch
    {
        private static readonly FieldInfo CachedPresetsField =
            AccessTools.Field(Type.GetType("MCM.UI.GUI.ViewModels.SettingsVM, MCMv3.UI.v3.1.9"), "_cachedPresets");
        private static readonly FieldInfo PresetsSelectorBackingField =
            AccessTools.Field(Type.GetType("MCM.UI.GUI.ViewModels.SettingsVM, MCMv3.UI.v3.1.9"), "<PresetsSelector>k__BackingField");
        private static readonly MethodInfo RecalculateIndexMethod =
            AccessTools.Method(Type.GetType("MCM.UI.GUI.ViewModels.SettingsVM, MCMv3.UI.v3.1.9"), "RecalculateIndex");

        public static void Postfix(object __instance)
        {
            var cachedPresets = (IDictionary<string, BaseSettings>) CachedPresetsField.GetValue(__instance);

            var presetsSelector = new SelectorVM<SelectorItemVM>(new List<string> { new TextObject("{=SettingsVM_Custom}Custom").ToString() }.Concat(cachedPresets.Keys.Select(x => new TextObject(x).ToString())), -1, null);
            presetsSelector.ItemList[0].CanBeSelected = false;
            PresetsSelectorBackingField.SetValue(__instance, presetsSelector);

            RecalculateIndexMethod.Invoke(__instance, Array.Empty<object>());
        }
    }
}
