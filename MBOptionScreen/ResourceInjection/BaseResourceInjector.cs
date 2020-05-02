using MBOptionScreen.Utils;

using System.Linq;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    public abstract class BaseResourceInjector
    {
        private static BaseResourceInjector? _instance;
        public static BaseResourceInjector Instance => _instance
            ?? (_instance = DI.GetImplementation<BaseResourceInjector, ResourceInjectorWrapper>(ApplicationVersionUtils.GameVersion()));

        public abstract void InjectBrush(XmlDocument xmlDocument);
        public abstract void InjectPrefab(string prefabName, XmlDocument xmlDocument);

        public WidgetPrefab? RequestMovie(string movie)
        {
            var loaders = DI.GetImplementations<IResourceLoader, ResourceLoaderWrapper>(ApplicationVersionUtils.GameVersion()).ToList();

            foreach (var resourceLoader in loaders)
            {
                var widget = resourceLoader.MovieRequested(movie);
                if (widget != null)
                    return widget;
            }
            return null;
        }
    }
}