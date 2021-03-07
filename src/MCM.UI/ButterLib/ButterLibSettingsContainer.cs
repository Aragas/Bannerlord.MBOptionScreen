using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;

using Microsoft.Extensions.Logging;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace MCM.UI.ButterLib
{
    internal sealed class ButterLibSettingsContainer : BaseSettingsContainer<BaseSettings>, IGlobalSettingsContainer
    {
        protected override string RootFolder { get; }

        public ButterLibSettingsContainer(ILogger<ButterLibSettingsContainer> logger)
        {
            RootFolder = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");

            RegisterSettings(new ButterLibSettings());
        }
    }
}