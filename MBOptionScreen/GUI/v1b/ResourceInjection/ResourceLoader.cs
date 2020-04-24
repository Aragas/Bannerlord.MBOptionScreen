using MBOptionScreen.Attributes;
using MBOptionScreen.ResourceInjection;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.GUI.v1b.ResourceInjection
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    internal class ResourceLoader : IResourceLoader
    {
        public WidgetPrefab? MovieRequested(string movie)
        {
            if (movie == "ModOptionsView_v1b")
            {
                return PrefabsLoader.LoadModOptionsView();
            }
            if (movie == "EditValueView_v1b")
            {
                return PrefabsLoader.LoadEditValueView();
            }

            return null;
        }
    }
}
