using HarmonyLib;

using MCM.Utils;

namespace MCM.UI.ResourceInjection.Harmony
{
    public abstract class BaseHarmonyPatches
    {
        private static BaseHarmonyPatches? _instance;
        public static BaseHarmonyPatches Instance
        {
            get
            {
                if (_instance == null)
                {
                    var version = ApplicationVersionUtils.GameVersion();
                    _instance = DI.GetImplementation<BaseHarmonyPatches, HarmonyPatchesWrapper>(version);
                }

                return _instance;
            }
        }

        public virtual HarmonyMethod? LoadMovie { get; }
        public virtual HarmonyMethod? CollectWidgetTypes { get; }
        public virtual HarmonyMethod? LoadBrushes { get; }
    }
}