using HarmonyLib;

using System.Reflection;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.Abstractions.ResourceInjection
{
    public sealed class ResourceLoaderWrapper : IResourceLoader, IWrapper
    {
        public object Object { get; }
        private MethodInfo? MovieRequestedMethod { get; }
        public bool IsCorrect { get; }

        public ResourceLoaderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            MovieRequestedMethod = AccessTools.Method(type, nameof(MovieRequested));

            IsCorrect = MovieRequestedMethod != null;
        }

        public WidgetPrefab? MovieRequested(string movie) => MovieRequestedMethod?.Invoke(Object, new object[] { movie }) as WidgetPrefab;
    }
}