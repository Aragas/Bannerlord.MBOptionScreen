using MBOptionScreen.Attributes;
using MBOptionScreen.ResourceInjection;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.GUI.v1e_7.ResourceInjection
{
    [Version("e1.0.0",  210)]
    [Version("e1.0.1",  210)]
    [Version("e1.0.2",  210)]
    [Version("e1.0.3",  210)]
    [Version("e1.0.4",  210)]
    [Version("e1.0.5",  210)]
    [Version("e1.0.6",  210)]
    [Version("e1.0.7",  210)]
    [Version("e1.0.8",  210)]
    [Version("e1.0.9",  210)]
    [Version("e1.0.10", 210)]
    [Version("e1.0.11", 210)]
    [Version("e1.1.0",  210)]
    [Version("e1.2.0",  210)]
    [Version("e1.2.1",  210)]
    [Version("e1.3.0",  210)]
    internal class ResourceLoader : IResourceLoader
    {
        public WidgetPrefab? MovieRequested(string movie)
        {
            if (movie == "ModOptionsView_v1e_7")
            {
                return PrefabsLoader.LoadModOptionsView();
            }
            if (movie == "EditValueView_v1e_7")
            {
                return PrefabsLoader.LoadEditValueView();
            }

            return null;
        }
    }
}
