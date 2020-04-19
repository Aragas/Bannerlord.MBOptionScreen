using MBOptionScreen.Interfaces;

using System.Reflection;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    internal class ResourceInjectorWrapper : IResourceInjector
    {
        private readonly object _object;
        private MethodInfo InjectBrushMethod { get; }
        private MethodInfo InjectPrefabMethod { get; }
        private MethodInfo RequestMovieMethod { get; }

        public ResourceInjectorWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();
            InjectBrushMethod = type.GetMethod("InjectBrush", BindingFlags.Instance | BindingFlags.Public);
            InjectPrefabMethod = type.GetMethod("InjectPrefab", BindingFlags.Instance | BindingFlags.Public);
            RequestMovieMethod = type.GetMethod("RequestMovie", BindingFlags.Instance | BindingFlags.Public);
        }

        public void InjectBrush(XmlDocument xmlDocument) =>
            InjectBrushMethod.Invoke(_object, new object[] { xmlDocument });
        public void InjectPrefab(string prefabName, XmlDocument xmlDocument) =>
            InjectPrefabMethod.Invoke(_object, new object[] { prefabName, xmlDocument });
        public WidgetPrefab? RequestMovie(string movie) =>
            RequestMovieMethod.Invoke(_object, new object[] { movie }) as WidgetPrefab;
    }
}