using HarmonyLib;

using System;
using System.Reflection;
using System.Xml;

namespace MCM.Abstractions.ResourceInjection
{
    public sealed class ResourceInjectorWrapper : BaseResourceInjector, IWrapper
    {
        public object Object { get; }
        private MethodInfo? InjectBrushMethod { get; }
        private MethodInfo? InjectPrefabMethod { get; }
        private MethodInfo? InjectWidgetMethod { get; }
        public bool IsCorrect { get; }

        public ResourceInjectorWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            InjectBrushMethod = AccessTools.Method(type, nameof(InjectBrush));
            InjectPrefabMethod = AccessTools.Method(type, nameof(InjectPrefab));
            InjectWidgetMethod = AccessTools.Method(type, nameof(InjectWidget));

            IsCorrect = InjectBrushMethod != null && InjectPrefabMethod != null && InjectWidgetMethod != null;
        }

        public override void InjectBrush(XmlDocument xmlDocument) =>
            InjectBrushMethod?.Invoke(Object, new object[] { xmlDocument });
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument) =>
            InjectPrefabMethod?.Invoke(Object, new object[] { prefabName, xmlDocument });
        public override void InjectWidget(Type widgetType) =>
            InjectWidgetMethod?.Invoke(Object, new object[] { widgetType });
    }
}