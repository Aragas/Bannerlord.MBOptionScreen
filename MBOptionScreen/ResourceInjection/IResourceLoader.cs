using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    public interface IResourceLoader
    {
        WidgetPrefab? MovieRequested(string movie);
    }
}