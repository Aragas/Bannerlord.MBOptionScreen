using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Base.Global;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM
{
    internal sealed class MCMSettings : AttributeGlobalSettings<MCMSettings>
    {
        public override string Id { get; } = "MCM_v3";
        public override string DisplayName => TextObjectHelper.Create("{=MCMSettings_Name}Mod Configuration Menu {VERSION}", new Dictionary<string, TextObject>()
        {
            { "VERSION", TextObjectHelper.Create(typeof(MCMSettings).Assembly.GetName().Version.ToString(3)) }
        }).ToString();
        public override string FolderName { get; } = "MCM";
        public override string Format { get; } = "memory";
    }
}