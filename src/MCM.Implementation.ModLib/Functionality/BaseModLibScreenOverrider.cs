using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace MCM.Implementation.ModLib.Functionality
{
    public abstract class BaseModLibScreenOverrider
    {
        public static BaseModLibScreenOverrider? Instance =>
            MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<BaseModLibScreenOverrider>();

        public abstract void OverrideModLibScreen();
    }
}