using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.Interfaces;
using MBOptionScreen.ResourceInjection.EmbedLoaders;
using MBOptionScreen.ResourceInjection.Injectors;

using System.Reflection;
using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Can be easily replaced once a less hacky solution is found
    /// </summary>
    [ResourceInjectorVersion("e1.0.0",  1)]
    [ResourceInjectorVersion("e1.0.1",  1)]
    [ResourceInjectorVersion("e1.0.2",  1)]
    [ResourceInjectorVersion("e1.0.3",  1)]
    [ResourceInjectorVersion("e1.0.4",  1)]
    [ResourceInjectorVersion("e1.0.5",  1)]
    [ResourceInjectorVersion("e1.0.6",  1)]
    [ResourceInjectorVersion("e1.0.7",  1)]
    [ResourceInjectorVersion("e1.0.8",  1)]
    [ResourceInjectorVersion("e1.0.9",  1)]
    [ResourceInjectorVersion("e1.0.10", 1)]
    [ResourceInjectorVersion("e1.0.11", 1)]
    [ResourceInjectorVersion("e1.1.0",  1)]
    internal class DefaultResourceInjector : IResourceInjector
    {
        private static PropertyInfo RootViewProperty { get; } =
            typeof(GauntletMovie).GetProperty("RootView", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private static FieldInfo MovieRootNodeField { get; } =
            typeof(GauntletMovie).GetField("_movieRootNode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private static bool LoadMoviePrefix(GauntletMovie __instance)
        {
            var movie = MBOptionScreenSubModule.SharedStateObject.ResourceInjector.RequestMovie(__instance.MovieName);
            if (movie == null)
                return true;

            var widgetCreationData = new WidgetCreationData(__instance.Context, __instance.WidgetFactory);
            widgetCreationData.AddExtensionData(__instance);
            var widgetInstantiationResult = movie.Instantiate(widgetCreationData);
            RootViewProperty.SetValue(__instance, widgetInstantiationResult.GetGauntletView());
            var target = __instance.RootView.Target;
            var movieRootNode = (Widget)MovieRootNodeField.GetValue(__instance);
            movieRootNode.AddChild(target);
            __instance.RootView.RefreshBindingWithChildren();
            return false;
        }

        public DefaultResourceInjector()
        {
            new Harmony("bannerlord.mboptionscreen.defaultresourceinjector").Patch(
                original: typeof(GauntletMovie).GetMethod("LoadMovie", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
                prefix: new HarmonyMethod(typeof(DefaultResourceInjector).GetMethod("LoadMoviePrefix", BindingFlags.Static | BindingFlags.NonPublic)));
        }

        public void InjectBrush(XmlDocument xmlDocument)
        {
            BrushInjector.InjectDocument(xmlDocument);
        }

        public void InjectPrefab(string prefabName, XmlDocument xmlDocument)
        {
            PrefabInjector.InjectDocumentAndCreate(prefabName, xmlDocument);
        }

        public WidgetPrefab? RequestMovie(string movie)
        {
            if (movie == "ModOptionsView_v1a")
            {
                return PrefabsLoaderV1a.LoadModOptionsView();
            }
            if (movie == "EditValueView_v1a")
            {
                return PrefabsLoaderV1a.LoadEditValueView();
            }


            return null;
        }
    }
}