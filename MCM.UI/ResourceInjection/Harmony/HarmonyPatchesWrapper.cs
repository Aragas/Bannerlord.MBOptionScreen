using HarmonyLib;

using MCM.Abstractions;

using System.Reflection;

namespace MCM.UI.ResourceInjection.Harmony
{
    internal sealed class HarmonyPatchesWrapper : BaseHarmonyPatches, IWrapper
    {
        private readonly object _object;
        private PropertyInfo LoadMovieProperty { get; }
        private PropertyInfo CollectWidgetTypesProperty { get; }
        private PropertyInfo LoadBrushesProperty { get; }
        public bool IsCorrect { get; }

        public override HarmonyMethod? LoadMovie => LoadMovieProperty.GetValue(_object) as HarmonyMethod;
        public override HarmonyMethod? CollectWidgetTypes => CollectWidgetTypesProperty.GetValue(_object) as HarmonyMethod;
        public override HarmonyMethod? LoadBrushes => LoadBrushesProperty.GetValue(_object) as HarmonyMethod;

        public HarmonyPatchesWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            LoadMovieProperty = AccessTools.Property(type, nameof(BaseHarmonyPatches.LoadMovie));
            CollectWidgetTypesProperty = AccessTools.Property(type, nameof(BaseHarmonyPatches.CollectWidgetTypes));
            LoadBrushesProperty = AccessTools.Property(type, nameof(BaseHarmonyPatches.LoadBrushes));

            IsCorrect = LoadMovieProperty != null && CollectWidgetTypesProperty != null && LoadBrushesProperty != null;
        }
    }
}