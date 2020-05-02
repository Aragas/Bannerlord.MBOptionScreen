using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.ResourceInjection.Injectors;

using System.Threading;
using System.Xml;

using TaleWorlds.GauntletUI.Data;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Can be easily replaced once a less hacky solution is found
    /// </summary>
    [Version("e1.0.0",  220)]
    [Version("e1.0.1",  220)]
    [Version("e1.0.2",  220)]
    [Version("e1.0.3",  220)]
    [Version("e1.0.4",  220)]
    [Version("e1.0.5",  220)]
    [Version("e1.0.6",  220)]
    [Version("e1.0.7",  220)]
    [Version("e1.0.8",  220)]
    [Version("e1.0.9",  220)]
    [Version("e1.0.10", 220)]
    [Version("e1.0.11", 220)]
    [Version("e1.1.0",  220)]
    [Version("e1.2.0",  220)]
    [Version("e1.2.1",  220)]
    [Version("e1.3.0",  220)]
    internal sealed class DefaultResourceInjector : BaseResourceInjector
    {
        private static int _initialized;

        public DefaultResourceInjector()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                new Harmony("bannerlord.mboptionscreen.defaultresourceinjector").Patch(
                    original: AccessTools.Method(typeof(GauntletMovie), "LoadMovie"),
                    prefix: BaseGauntletMoviePatches.Instance.LoadMoviePostfix);
            }
        }

        public override void InjectBrush(XmlDocument xmlDocument)
        {
            BrushInjector.InjectDocument(xmlDocument);
        }

        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument)
        {
            PrefabInjector.InjectDocumentAndCreate(prefabName, xmlDocument);
        }
    }
}