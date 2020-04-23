using MBOptionScreen.Utils;

using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection 
{
    public abstract class BaseResourceInjector
    {
        private static BaseResourceInjector? _instance;
        public static BaseResourceInjector Instance => _instance
            ?? (_instance = ReflectionUtils.GetImplementation<BaseResourceInjector, ResourceInjectorWrapper>(ApplicationVersionParser.GameVersion()));

        public abstract void InjectBrush(XmlDocument xmlDocument);
        public abstract void InjectPrefab(string prefabName, XmlDocument xmlDocument);

        public abstract WidgetPrefab? RequestMovie(string movie);
    }
}