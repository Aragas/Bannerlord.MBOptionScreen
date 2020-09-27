using MCM.Abstractions.Settings.Base.Global;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Implementation
{
    internal sealed class MCMImplementationSettings : AttributeGlobalSettings<MCMImplementationSettings>
    {
        public override string Id { get; } = "MCMImplementation_v3";
        public override string DisplayName => new TextObject("{=MCMImplementationSettings_Name}MCM Impl. {VERSION}", new Dictionary<string, TextObject>
        {
            { "VERSION", new TextObject(typeof(MCMImplementationSettings).Assembly.GetName().Version.ToString(3)) }
        }).ToString();
        public override string FolderName { get; } = "MCM";
        public override string FormatType { get; } = "memory";
    }
}