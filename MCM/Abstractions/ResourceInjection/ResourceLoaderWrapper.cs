using HarmonyLib;

using System.Reflection;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.Abstractions.ResourceInjection
{
    public class ResourceLoaderWrapper : IResourceLoader, IWrapper
    {
        private readonly object _object;
        private MethodInfo? MovieRequestedMethod { get; }
        public bool IsCorrect { get; }

        public ResourceLoaderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            MovieRequestedMethod = AccessTools.Method(type, nameof(MovieRequested));

            IsCorrect = MovieRequestedMethod != null;
        }

        public WidgetPrefab? MovieRequested(string movie) => MovieRequestedMethod?.Invoke(_object, new object[] { movie }) as WidgetPrefab;
    }
}