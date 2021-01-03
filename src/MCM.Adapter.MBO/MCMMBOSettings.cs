extern alias v4;

using TaleWorlds.Localization;

namespace MCM.Adapter.MBO
{
    internal sealed class MCMMBOSettings : v4::MCM.Abstractions.Settings.Base.Global.AttributeGlobalSettings<MCMMBOSettings>
    {
        public override string Id { get; } = "MCMMBO_v3";
        public override string DisplayName => new TextObject("{=MCMMBOSettings_Name}MCM MBO Adapter {VERSION}", new()
        {
            { "VERSION", new TextObject(typeof(MCMMBOSettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR") }
        }).ToString();
        public override string FolderName { get; } = "MCM";
        public override string FormatType { get; } = "none";
    }
}