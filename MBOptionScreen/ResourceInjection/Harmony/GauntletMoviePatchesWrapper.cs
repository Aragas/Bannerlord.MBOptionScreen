using HarmonyLib;

using System.Reflection;

namespace MBOptionScreen.ResourceInjection
{
    internal sealed class GauntletMoviePatchesWrapper : BaseGauntletMoviePatches
    {
        private readonly object _object;

        private PropertyInfo LoadMoviePostfixProperty { get; }

        public override HarmonyMethod? LoadMoviePostfix => LoadMoviePostfixProperty.GetValue(_object) as HarmonyMethod;

        public GauntletMoviePatchesWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            LoadMoviePostfixProperty = AccessTools.Property(type, "LoadMoviePostfix");
        }
    }
}