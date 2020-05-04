using MCM.Utils;

using System;
using System.Linq;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.Abstractions.ResourceInjection
{
    public abstract class BaseResourceInjector
    {
        private static BaseResourceInjector? _instance;
        public static BaseResourceInjector Instance => _instance
            ?? (_instance = DI.GetImplementation<BaseResourceInjector, ResourceInjectorWrapper>(ApplicationVersionUtils.GameVersion()));

        public abstract void InjectBrush(XmlDocument xmlDocument);
        public abstract void InjectPrefab(string prefabName, XmlDocument xmlDocument);
        public abstract void InjectWidget(Type widgetType);

        public WidgetPrefab? RequestMovie(string movie) => DI.GetImplementations<IResourceLoader, ResourceLoaderWrapper>(ApplicationVersionUtils.GameVersion())
            .Select(resourceLoader => resourceLoader.MovieRequested(movie))
            .FirstOrDefault(widget => widget != null);
    }
}