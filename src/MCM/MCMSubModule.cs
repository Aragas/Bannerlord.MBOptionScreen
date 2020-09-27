using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.Extensions;

using TaleWorlds.MountAndBlade;

namespace MCM
{
    public sealed class MCMSubModule : MBSubModuleBase
    {
        public static MCMSubModule? Instance { get; private set; }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Instance = this;

            var services = this.GetServices();

            services.AddSettingsFormat<MemorySettingsFormat>();
            services.AddSettingsPropertyDiscoverer<NoneSettingsPropertyDiscoverer>();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            Instance = null;
        }
    }
}