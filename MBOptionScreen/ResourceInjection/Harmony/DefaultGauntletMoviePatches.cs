using HarmonyLib;

using MBOptionScreen.Attributes;

using System.Reflection;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
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
    [Version("e1.2.1",  200)]
    [Version("e1.3.0",  200)]
    internal sealed class DefaultGauntletMoviePatches : BaseGauntletMoviePatches
    {
        private static PropertyInfo RootViewProperty { get; } = AccessTools.Property(typeof(GauntletMovie), "RootView");
        private static bool LoadMovieHarmonyPostfix(GauntletMovie __instance, Widget ____movieRootNode)
        {
            var movie = BaseResourceInjector.Instance.RequestMovie(__instance.MovieName);
            if (movie == null)
                return true;

            var widgetCreationData = new WidgetCreationData(__instance.Context, __instance.WidgetFactory);
            widgetCreationData.AddExtensionData(__instance);
            RootViewProperty.SetValue(__instance, movie.Instantiate(widgetCreationData).GetGauntletView());
            ____movieRootNode.AddChild(__instance.RootView.Target);
            __instance.RootView.RefreshBindingWithChildren();
            return false;
        }

        public override HarmonyMethod? LoadMoviePostfix => new HarmonyMethod(AccessTools.Method(typeof(DefaultGauntletMoviePatches), nameof(LoadMovieHarmonyPostfix)));
    }
}