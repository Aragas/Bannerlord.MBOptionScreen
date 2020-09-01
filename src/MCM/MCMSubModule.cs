using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Formats.Memory;
using MCM.Extensions;

using TaleWorlds.MountAndBlade;

namespace MCM
{
    public sealed class MCMSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var services = this.GetServices();

            services.AddSettingsFormat<IMemorySettingsFormat, MemorySettingsFormat>();
        }
    }
}