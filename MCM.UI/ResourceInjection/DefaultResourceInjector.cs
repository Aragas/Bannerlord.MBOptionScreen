using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.ResourceInjection;
using MCM.UI.ResourceInjection.Harmony;
using MCM.UI.ResourceInjection.Injectors;

using System;
using System.Threading;
using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;

namespace MCM.UI.ResourceInjection
{
    /// <summary>
    /// Can be easily replaced once a less hacky solution is found
    /// </summary>
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    internal sealed class DefaultResourceInjector : BaseResourceInjector
    {
        private static int _initialized;

        public DefaultResourceInjector()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                new HarmonyLib.Harmony("bannerlord.mcm.loadmovie_v3").Patch(
                    original: AccessTools.Method(typeof(GauntletMovie), "LoadMovie"),
                    prefix: BaseHarmonyPatches.Instance.LoadMovie);

                new HarmonyLib.Harmony("bannerlord.mcm.loadbrush_v3").Patch(
                    original: AccessTools.Method(typeof(BrushFactory), "LoadBrushes"),
                    postfix: BaseHarmonyPatches.Instance.LoadBrushes);
            }
        }

        public override void InjectBrush(XmlDocument xmlDocument) =>
            BrushInjector.InjectDocument(xmlDocument);
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument) =>
            PrefabInjector.InjectDocumentAndCreate(prefabName, xmlDocument);
        public override void InjectWidget(Type widgetType) =>
            WidgetInjector.InjectWidget(widgetType);
    }
}