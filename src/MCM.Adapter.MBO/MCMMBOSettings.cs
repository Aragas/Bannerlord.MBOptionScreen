extern alias v4;

using Bannerlord.BUTR.Shared.Helpers;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Adapter.MBO
{
    internal sealed class MCMMBOSettings : v4::MCM.Abstractions.Settings.Base.Global.AttributeGlobalSettings<MCMMBOSettings>
    {
        public override string Id { get; } = "MCMMBO_v3";
        public override string DisplayName => TextObjectHelper.Create("{=MCMMBOSettings_Name}MCM MBO Adapter {VERSION}", new Dictionary<string, TextObject>
        {
            { "VERSION", TextObjectHelper.Create(typeof(MCMMBOSettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR") }
        })?.ToString() ?? "ERROR";
        public override string FolderName { get; } = "MCM";
        public override string FormatType { get; } = "none";
    }
}