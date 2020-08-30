using HarmonyLib;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal static class WidgetInjector
    {
        private static readonly AccessTools.FieldRef<WidgetFactory, Dictionary<string, Type>>? GetBuiltinTypes =
            AccessTools.FieldRefAccess<WidgetFactory, Dictionary<string, Type>>("_builtinTypes");

        internal static readonly ConcurrentDictionary<Type, object?> BuiltinWidgets = new ConcurrentDictionary<Type, object?>();

        public static void InjectWidget(Type widgetType)
        {
            if (GetBuiltinTypes != null)
            {
                var builtinTypes = GetBuiltinTypes(UIResourceManager.WidgetFactory);
                builtinTypes[widgetType.Name] = widgetType;
                BuiltinWidgets.TryAdd(widgetType, null);
            }
        }
    }
}