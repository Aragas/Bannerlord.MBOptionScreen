using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.Abstractions.ResourceInjection
{
    public interface IResourceLoader
    {
        WidgetPrefab? MovieRequested(string movie);
    }
}