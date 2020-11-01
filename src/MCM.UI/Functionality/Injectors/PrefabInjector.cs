using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.UI.Patches;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal static class PrefabInjector
    {
        private static Type CustomWidgetType { get; } =
            typeof(WidgetTemplate).Assembly.GetType("TaleWorlds.GauntletUI.PrefabSystem.CustomWidgetType");

        private delegate void SetNameDelegate(object instance, string name);
        private delegate void SetWidgetFactoryDelegate(object instance, WidgetFactory widgetFactory);
        private delegate void SetWidgetPrefabDelegate(object instance, WidgetPrefab widgetPrefab);

        private static readonly SetNameDelegate? SetName =
            AccessTools2.GetDelegateObjectInstance<SetNameDelegate>(AccessTools.Property(CustomWidgetType, "Name").SetMethod);
        private static readonly SetWidgetFactoryDelegate? SetWidgetFactory =
            AccessTools2.GetDelegateObjectInstance<SetWidgetFactoryDelegate>(AccessTools.Property(CustomWidgetType, "WidgetFactory").SetMethod);
        private static readonly SetWidgetPrefabDelegate? SetWidgetPrefab =
            AccessTools2.GetDelegateObjectInstance<SetWidgetPrefabDelegate>(AccessTools.Property(CustomWidgetType, "WidgetPrefab").SetMethod);
        private static readonly AccessTools.FieldRef<object, string>? GetResourcesPath =
            AccessTools2.FieldRefAccess<string>(CustomWidgetType, "_resourcesPath");
        private static readonly AccessTools.FieldRef<object, IDictionary>? GetCustomTypes =
            AccessTools2.FieldRefAccess<IDictionary>(typeof(WidgetFactory), "_customTypes");
        private static readonly AccessTools.FieldRef<object, IDictionary>? GetCustomTypePaths =
            AccessTools2.FieldRefAccess<IDictionary>(typeof(WidgetFactory), "_customTypePaths");
        private static readonly AccessTools.FieldRef<object, IDictionary>? GetLiveCustomTypes =
            AccessTools2.FieldRefAccess<IDictionary>(typeof(WidgetFactory), "_liveCustomTypes");
        private static readonly AccessTools.FieldRef<object, Dictionary<string, int>>? GetLiveInstanceTracker =
            AccessTools2.FieldRefAccess<Dictionary<string, int>>(typeof(WidgetFactory), "_liveInstanceTracker");

        public static void Register(string name)
        {
            if (GetCustomTypePaths is { })
            {
                var dict = GetCustomTypePaths(UIResourceManager.WidgetFactory);
                if (!dict.Contains(name))
                    dict.Add(name, "");
            }
        }

        public static WidgetPrefab Create(string name, XmlDocument doc)
        {
            var widgetPrefab = WidgetPrefabPatch.LoadFromDocument(
                UIResourceManager.WidgetFactory.PrefabExtensionContext,
                UIResourceManager.WidgetFactory.WidgetAttributeContext,
                string.Empty,
                doc);

            var customWidgetType = FormatterServices.GetUninitializedObject(CustomWidgetType);
            if (GetResourcesPath is { }) GetResourcesPath(customWidgetType) = string.Empty;
            SetName?.Invoke(customWidgetType, name);
            SetWidgetFactory?.Invoke(customWidgetType, UIResourceManager.WidgetFactory);
            SetWidgetPrefab?.Invoke(customWidgetType, widgetPrefab);

            return widgetPrefab;
        }

        // Pre154
        public static WidgetPrefab CreateAndInject(string name, XmlDocument doc)
        {
            var widgetPrefab = WidgetPrefabPatch.LoadFromDocument(
                UIResourceManager.WidgetFactory.PrefabExtensionContext,
                UIResourceManager.WidgetFactory.WidgetAttributeContext,
                string.Empty,
                doc);

            var customWidgetType = FormatterServices.GetUninitializedObject(CustomWidgetType);
            if (GetResourcesPath is { }) GetResourcesPath(customWidgetType) = string.Empty;
            SetName?.Invoke(customWidgetType, name);
            SetWidgetFactory?.Invoke(customWidgetType, UIResourceManager.WidgetFactory);
            SetWidgetPrefab?.Invoke(customWidgetType, widgetPrefab);

            if (GetCustomTypes is { })
            {
                var dict = GetCustomTypes(UIResourceManager.WidgetFactory);
                if (!dict.Contains(name))
                    dict.Add(name, customWidgetType);
            }

            return widgetPrefab;
        }
    }
}