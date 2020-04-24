using HarmonyLib;

using System.Reflection;
using System.Xml;

namespace MBOptionScreen.ResourceInjection
{
    internal sealed class ResourceInjectorWrapper : BaseResourceInjector, IWrapper
    {
        private readonly object _object;
        private MethodInfo InjectBrushMethod { get; }
        private MethodInfo InjectPrefabMethod { get; }
        public bool IsCorrect { get; }

        public ResourceInjectorWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            InjectBrushMethod = AccessTools.Method(type, "InjectBrush");
            InjectPrefabMethod = AccessTools.Method(type, "InjectPrefab");

            IsCorrect = InjectBrushMethod != null && InjectPrefabMethod != null;
        }

        public override void InjectBrush(XmlDocument xmlDocument) =>
            InjectBrushMethod.Invoke(_object, new object[] { xmlDocument });
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument) =>
            InjectPrefabMethod.Invoke(_object, new object[] { prefabName, xmlDocument });
    }
}