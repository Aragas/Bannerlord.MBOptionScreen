using HarmonyLib;

using MBOptionScreen.Utils;

namespace MBOptionScreen.ResourceInjection
{
    public abstract class BaseGauntletMoviePatches
    {
        private static BaseGauntletMoviePatches? _instance;
        public static BaseGauntletMoviePatches Instance
        {
            get
            {
                if (_instance == null)
                {
                    var version = ApplicationVersionUtils.GameVersion();
                    _instance = DI.GetImplementation<BaseGauntletMoviePatches, GauntletMoviePatchesWrapper>(version);
                }

                return _instance;
            }
        }

        public virtual HarmonyMethod? LoadMoviePostfix { get; }
    }
}