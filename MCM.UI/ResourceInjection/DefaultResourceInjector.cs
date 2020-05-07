using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.ResourceInjection;
using MCM.UI.ResourceInjection.Injectors;

using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

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
                var harmony = new Harmony("bannerlord.mcm.recourceinjector_v3");
                harmony.Patch(
                    original: AccessTools.Method(typeof(GauntletMovie), "LoadMovie"),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(DefaultResourceInjector), nameof(LoadMovieHarmony))));
                harmony.Patch(
                    original: AccessTools.Method(typeof(BrushFactory), "LoadBrushes"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultResourceInjector), nameof(LoadBrushesHarmony))));
            }
        }

        private static PropertyInfo RootViewProperty { get; } = AccessTools.Property(typeof(GauntletMovie), nameof(GauntletMovie.RootView));
        private static bool LoadMovieHarmony(GauntletMovie __instance, Widget ____movieRootNode)
        {
            var movie = RequestMovie(__instance.MovieName);
            if (movie == null)
                return true;

            var widgetCreationData = new WidgetCreationData(__instance.Context, __instance.WidgetFactory);
            widgetCreationData.AddExtensionData(__instance);
            RootViewProperty.SetValue(__instance, movie.Instantiate(widgetCreationData).GetGauntletView());
            ____movieRootNode.AddChild(__instance.RootView.Target);
            __instance.RootView.RefreshBindingWithChildren();
            return false;
        }

        private static FieldInfo BrushesField { get; } = AccessTools.Field(typeof(BrushFactory), "_brushes");
        private static void LoadBrushesHarmony(BrushFactory __instance)
        {
            var brushes = (IDictionary) BrushesField.GetValue(__instance);
            foreach (var brush in BrushInjector._brushes.Keys)
            {
                if (brushes.Contains(brush.Name))
                    brushes[brush.Name] = brush;
                else
                    brushes.Add(brush.Name, brush);
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