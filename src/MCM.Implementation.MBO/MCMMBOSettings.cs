extern alias v4;

using System.Collections.Generic;

using TaleWorlds.Localization;

using v4::MCM.Abstractions.Settings.Base.Global;

namespace MCM.Implementation.MBO
{
    internal sealed class MCMMBOSettings : AttributeGlobalSettings<MCMMBOSettings>
    {
        public override string Id { get; } = "MCMMBO_v3";
        public override string DisplayName => new TextObject("{=MCMMBOSettings_Name}MCM MBO Impl. {VERSION}", new Dictionary<string, TextObject>()
        {
            { "VERSION", new TextObject(typeof(MCMMBOSettings).Assembly.GetName().Version.ToString(3)) }
        }).ToString();
        public override string FolderName { get; } = "MCM";
        public override string FormatType { get; } = "none";
    }
}