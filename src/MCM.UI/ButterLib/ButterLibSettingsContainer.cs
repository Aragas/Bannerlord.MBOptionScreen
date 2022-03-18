using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib;

using MCM.Abstractions.Dropdown;
using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.UI.Extensions;

using Microsoft.Extensions.Logging;

using TaleWorlds.Localization;

using Path = System.IO.Path;

namespace MCM.UI.ButterLib
{
    internal sealed class ButterLibSettingsContainer : BaseSettingsContainer<BaseSettings>, IGlobalSettingsContainer
    {
        protected override string RootFolder { get; }

        public ButterLibSettingsContainer(ILogger<ButterLibSettingsContainer> logger)
        {
            RootFolder = Path.Combine(FSIOHelper.GetConfigPath()!, "ModSettings");

            var prop = new StorageRef<DropdownDefault<string>>(new(new[]
            {
                $"{{=2Tp85Cpa}}{LogLevel.Trace}",
                $"{{=Es0LPYu1}}{LogLevel.Debug}",
                $"{{=fgLroxa7}}{LogLevel.Information}",
                $"{{=yBflFuRG}}{LogLevel.Warning}",
                $"{{=7tpjjYSV}}{LogLevel.Error}",
                $"{{=CarGIPlL}}{LogLevel.Critical}",
                $"{{=T3FtC5hh}}{LogLevel.None}"
            }, 2));
            var displayName = new TextObject("{=ButterLibSettings_Name}ButterLib {VERSION}", new()
            {
                { "VERSION", typeof(ButterLibSubModule).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
            }).ToString();
            var settings = BaseSettingsBuilder.Create("Options", displayName)?.SetFolderName("ButterLib").SetFormat("json2")
                .CreateGroup("{=ButterLibSettings_Name_Logging}Logging", builder =>
                    builder.AddDropdown("MinLogLevel", "{=ButterLibSettings_Name_LogLevel}Log Level", 0, prop, dBuilder =>
                        dBuilder.SetOrder(1).SetRequireRestart(true).SetHintText("{=ButterLibSettings_Name_LogLevelDesc}Level of logs to write.")))
                .AddButterLibSubSystems()
                .BuildAsGlobal();
            RegisterSettings(settings);
        }
    }
}