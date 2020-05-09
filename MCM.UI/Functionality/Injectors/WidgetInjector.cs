using HarmonyLib;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal static class WidgetInjector
    {
        private static FieldInfo BuiltinTypesField { get; } = AccessTools.Field(typeof(WidgetFactory), "_builtinTypes");

        internal static readonly ConcurrentDictionary<Type, object?> _builtinWidgets = new ConcurrentDictionary<Type, object?>();

        public static void InjectWidget(Type widgetType)
        {
            var builtinTypes = (IDictionary) BuiltinTypesField.GetValue(UIResourceManager.WidgetFactory);

            if (builtinTypes.Contains(widgetType.Name))
                builtinTypes[widgetType.Name] = widgetType;
            else
                builtinTypes.Add(widgetType.Name, widgetType);

            _builtinWidgets.TryAdd(widgetType, null);
        }
    }
}