using HarmonyLib;

using System.Reflection;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    internal sealed class ResourceInjectorWrapper : BaseResourceInjector
    {
        private readonly object _object;
        private MethodInfo InjectBrushMethod { get; }
        private MethodInfo InjectPrefabMethod { get; }
        private MethodInfo RequestMovieMethod { get; }

        public ResourceInjectorWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();
            InjectBrushMethod = AccessTools.Method(type, "InjectBrush");
            InjectPrefabMethod = AccessTools.Method(type, "InjectPrefab");
            RequestMovieMethod = AccessTools.Method(type, "RequestMovie");
        }

        public override void InjectBrush(XmlDocument xmlDocument) =>
            InjectBrushMethod.Invoke(_object, new object[] { xmlDocument });
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument) =>
            InjectPrefabMethod.Invoke(_object, new object[] { prefabName, xmlDocument });
        public override WidgetPrefab? RequestMovie(string movie) =>
            RequestMovieMethod.Invoke(_object, new object[] { movie }) as WidgetPrefab;
    }
}