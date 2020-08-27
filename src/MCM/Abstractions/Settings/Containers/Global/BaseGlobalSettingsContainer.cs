using MCM.Abstractions.Settings.Base.Global;

using System.IO;

namespace MCM.Abstractions.Settings.Containers.Global
{
    public abstract class BaseGlobalSettingsContainer : BaseSettingsContainer<GlobalSettings>, IGlobalSettingsContainer
    {
        /// <inheritdoc/>
        protected override string RootFolder { get; }

        protected BaseGlobalSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "Global");
        }
    }
}