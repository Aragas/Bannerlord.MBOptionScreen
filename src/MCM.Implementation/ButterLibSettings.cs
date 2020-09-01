using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Properties;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Implementation
{
    internal sealed class ButterLibSettings : BaseSettings
    {
        protected override ISettingsPropertyDiscoverer? Discoverer =>
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IAttributeSettingsPropertyDiscoverer>();

        public override string Id { get; } = "Options";
        public override string DisplayName => new TextObject("{=ButterLibSettings_Name}ButterLib {VERSION}", new Dictionary<string, TextObject>
        {
            { "VERSION", new TextObject(typeof(ButterLibSubModule).Assembly.GetName().Version.ToString(3)) }
        }).ToString();
        public override string FolderName { get; } = "ButterLib";
        public override string Format { get; } = "json2";

        [SettingPropertyDropdown("{=ButterLibSettings_Name_LogLevel}Log Level", Order = 1, RequireRestart = true, HintText = "{=ButterLibSettings_Name_LogLevelDesc}Level of logs to write.")]
        [SettingPropertyGroup("{=ButterLibSettings_Name_Logging}Logging")]
        public DropdownDefault<string> MinLogLevel { get; set; } = new DropdownDefault<string>(new[]
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