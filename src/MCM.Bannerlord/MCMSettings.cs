using MCM.Abstractions.Base.Global;

using TaleWorlds.Localization;

namespace MCM.Internal
{
    internal sealed class MCMSettings : AttributeGlobalSettings<MCMSettings>
    {
        /// <inheritdoc/>
        public override string Id => "MCM_v5";

        /// <inheritdoc/>
        public override string DisplayName => new TextObject("{=MCMSettings_Name}Mod Configuration Menu {VERSION}", new()
        {
            { "VERSION", typeof(MCMSettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
        }).ToString() ?? "ERROR";
        /// <inheritdoc/>
        public override string FolderName => "MCM";

        /// <inheritdoc/>
        public override string FormatType => "none";
    }
}