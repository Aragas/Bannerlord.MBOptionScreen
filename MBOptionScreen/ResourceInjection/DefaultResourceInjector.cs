using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.ResourceInjection.EmbedLoaders;
using MBOptionScreen.ResourceInjection.Injectors;

using System.Xml;

using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Can be easily replaced once a less hacky solution is found
    /// </summary>
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
    internal sealed class DefaultResourceInjector : BaseResourceInjector
    {
        public DefaultResourceInjector()
        {
            new Harmony("bannerlord.mboptionscreen.defaultresourceinjector").Patch(
                original: AccessTools.Method(typeof(GauntletMovie), "LoadMovie"),
                prefix: BaseGauntletMoviePatches.Instance.LoadMoviePostfix);
        }

        public override void InjectBrush(XmlDocument xmlDocument)
        {
            BrushInjector.InjectDocument(xmlDocument);
        }

        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument)
        {
            PrefabInjector.InjectDocumentAndCreate(prefabName, xmlDocument);
        }

        public override WidgetPrefab? RequestMovie(string movie)
        {
            if (movie == "ModOptionsView_v1b")
            {
                return PrefabsLoaderv1b.LoadModOptionsView();
            }
            if (movie == "EditValueView_v1b")
            {
                return PrefabsLoaderv1b.LoadEditValueView();
            }

            return null;
        }
    }
}