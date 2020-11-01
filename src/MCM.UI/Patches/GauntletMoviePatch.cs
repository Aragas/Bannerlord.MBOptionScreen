using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System;
using System.Runtime.CompilerServices;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Patches
{
    internal static class GauntletMoviePatch
    {
        private delegate void SetRootViewDelegate(GauntletMovie instance, GauntletView value);
        private static readonly SetRootViewDelegate? SetRootView =
            AccessTools2.GetDelegate<SetRootViewDelegate>(SymbolExtensions2.GetPropertyInfo((GauntletMovie gm) => gm.RootView).SetMethod);

        private static readonly AccessTools.FieldRef<GauntletMovie, WidgetPrefab>? MoviePrefab =
            AccessTools2.FieldRefAccess<GauntletMovie, WidgetPrefab>("_moviePrefab");

        private static Func<string, WidgetPrefab?>? _movieRequested;

        public static void Patch(Harmony harmony, Func<string, WidgetPrefab?> movieRequested)
        {
            _movieRequested = movieRequested;
            harmony.Patch(
                AccessTools.Method(typeof(GauntletMovie), "LoadMovie"),
                prefix: new HarmonyMethod(AccessTools.Method(typeof(GauntletMoviePatch), nameof(LoadMovieHarmony))));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool LoadMovieHarmony(GauntletMovie __instance, Widget ____movieRootNode)
        {
            var movie = _movieRequested?.Invoke(__instance.MovieName);
            if (movie is null)
                return true;

            if (MoviePrefab is { })
                MoviePrefab(__instance) = movie;

            var widgetCreationData = new WidgetCreationData(__instance.Context, __instance.WidgetFactory);
            widgetCreationData.AddExtensionData(__instance);
            SetRootView?.Invoke(__instance, movie.Instantiate(widgetCreationData).GetGauntletView());
            ____movieRootNode.AddChild(__instance.RootView.Target);
            __instance.RootView.RefreshBindingWithChildren();
            return false;
        }
    }
}