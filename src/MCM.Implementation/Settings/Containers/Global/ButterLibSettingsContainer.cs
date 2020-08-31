using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;

using Microsoft.Extensions.Logging;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace MCM.Implementation.Settings.Containers.Global
{
    internal sealed class ButterLibSettingsContainer : BaseSettingsContainer<GlobalSettings>, IGlobalSettingsContainer
    {
        protected override string RootFolder { get; }

        public ButterLibSettingsContainer(ILogger<ButterLibSettingsContainer> logger)
        {
            RootFolder = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");

            RegisterSettings(new ButterLibSettings());
        }
    }
}