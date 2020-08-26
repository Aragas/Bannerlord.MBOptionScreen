using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace MCM.Implementation.ModLib.Functionality
{
    public abstract class BaseModLibScreenOverrider
    {
        private static BaseModLibScreenOverrider? _instance;
        public static BaseModLibScreenOverrider Instance => _instance ??=
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<BaseModLibScreenOverrider>();

        public abstract void OverrideModLibScreen();
    }
}