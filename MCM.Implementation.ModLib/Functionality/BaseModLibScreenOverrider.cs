using MCM.Utils;

namespace MCM.Implementation.ModLib.Functionality
{
    public abstract class BaseModLibScreenOverrider
    {
        private static BaseModLibScreenOverrider? _instance;
        public static BaseModLibScreenOverrider Instance =>
            _instance ??= DI.GetImplementation<BaseModLibScreenOverrider, ModLibScreenOverriderWrapper>(ApplicationVersionUtils.GameVersion())!;

        public abstract void OverrideModLibScreen();
    }
}