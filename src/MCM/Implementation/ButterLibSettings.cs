using Bannerlord.ButterLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Settings.Base;

using Microsoft.Extensions.Logging;

using TaleWorlds.Localization;

namespace MCM.Implementation
{
    internal sealed class ButterLibSettings : BaseSettings
    {
        public override string Id { get; } = "Options";
        public override string DisplayName => new TextObject("{=ButterLibSettings_Name}ButterLib {VERSION}", new()
        {
            { "VERSION", new TextObject(typeof(ButterLibSubModule).Assembly.GetName().Version?.ToString(3) ?? "ERROR") }
        }).ToString();
        public override string FolderName { get; } = "ButterLib";
        public override string FormatType { get; } = "json2";
        public override string DiscoveryType { get; } = "attributes";

        [SettingPropertyDropdown("{=ButterLibSettings_Name_LogLevel}Log Level", Order = 1, RequireRestart = true, HintText = "{=ButterLibSettings_Name_LogLevelDesc}Level of logs to write.")]
        [SettingPropertyGroup("{=ButterLibSettings_Name_Logging}Logging")]
        public DropdownDefault<string> MinLogLevel { get; set; } = new(new[]
        {
            $"{{=2Tp85Cpa}}{LogLevel.Trace}",
            $"{{=Es0LPYu1}}{LogLevel.Debug}",
            $"{{=fgLroxa7}}{LogLevel.Information}",
            $"{{=yBflFuRG}}{LogLevel.Warning}",
            $"{{=7tpjjYSV}}{LogLevel.Error}",
            $"{{=CarGIPlL}}{LogLevel.Critical}",
            $"{{=T3FtC5hh}}{LogLevel.None}"
        }, 2);
    }
}