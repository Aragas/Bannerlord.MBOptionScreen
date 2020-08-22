using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions;
using MCM.Extensions;

namespace MCM.Implementation.ModLib.Functionality
{
    public abstract class BaseModLibScreenOverrider : IDependency
    {
        private static BaseModLibScreenOverrider? _instance;
        public static BaseModLibScreenOverrider Instance => _instance ??=
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<BaseModLibScreenOverrider, ModLibScreenOverriderWrapper>();
            //DI.GetImplementation<BaseModLibScreenOverrider, ModLibScreenOverriderWrapper>()!;

        public abstract void OverrideModLibScreen();
    }
}