using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    public interface IResourceInjector
    {
        void InjectBrush(XmlDocument xmlDocument);
        void InjectPrefab(string prefabName, XmlDocument xmlDocument);

        WidgetPrefab? RequestMovie(string movie);
    }
}