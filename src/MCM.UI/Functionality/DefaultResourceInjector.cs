using HarmonyLib;

using MCM.Abstractions.Functionality;
using MCM.UI.Functionality.Injectors;
using MCM.UI.Functionality.Loaders;

using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality
{
    /// <summary>
    /// Can be easily replaced once a less hacky solution is found
    /// </summary>
    internal sealed class DefaultResourceInjector : BaseResourceHandler
    {
        private static int _initialized;

        public DefaultResourceInjector()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                var harmony = new Harmony("bannerlord.mcm.recourceinjector");
                harmony.Patch(
                    AccessTools.Method(typeof(GauntletMovie), "LoadMovie"),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(DefaultResourceInjector), nameof(LoadMovieHarmony))));
                harmony.Patch(
                    AccessTools.Method(typeof(BrushFactory), "LoadBrushes"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(DefaultResourceInjector), nameof(LoadBrushesHarmony))));
            }
        }

        private static PropertyInfo RootViewProperty { get; } = AccessTools.Property(typeof(GauntletMovie), nameof(GauntletMovie.RootView));
        private static bool LoadMovieHarmony(GauntletMovie __instance, Widget ____movieRootNode)
        {
            var movie = Instance.MovieRequested(__instance.MovieName);
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
        public override WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCMv3" => PrefabsLoader.LoadModOptionsView_MCMv3(),
            "EditValueView_MCMv3" => PrefabsLoader.LoadEditValueView_MCMv3(),
            "OptionsWithModOptionsView_MCMv3" => PrefabsLoader.LoadOptionsWithModOptionsView_MCMv3(),
            _ => null
        };
    }
}