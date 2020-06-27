using MCM.Abstractions;
using MCM.Utils;

namespace MCM.Implementation.ModLib.Functionality
{
    public abstract class BaseModLibScreenOverrider : IDependency
    {
        private static BaseModLibScreenOverrider? _instance;
        public static BaseModLibScreenOverrider Instance =>
            _instance ??= DI.GetImplementation<BaseModLibScreenOverrider, ModLibScreenOverriderWrapper>()!;

        public abstract void OverrideModLibScreen();
    }
}