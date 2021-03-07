using MCM.DependencyInjection;

namespace MCM.Adapter.ModLib.Functionality
{
    public abstract class BaseModLibScreenOverrider
    {
        public static BaseModLibScreenOverrider? Instance => GenericServiceProvider.GetService<BaseModLibScreenOverrider>();

        public abstract void OverrideModLibScreen();
    }
}