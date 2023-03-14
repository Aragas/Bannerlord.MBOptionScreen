using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib;

using BUTR.DependencyInjection;

using MCM.Abstractions.Base;
using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.Global;
using MCM.Common;
using MCM.Implementation;
using MCM.UI.Extensions;

using Microsoft.Extensions.Logging;

using TaleWorlds.Engine;
using TaleWorlds.Localization;

using Path = System.IO.Path;

namespace MCM.UI.ButterLib
{
    internal sealed class ButterLibSettingsContainer : BaseSettingsContainer<BaseSettings>, IGlobalSettingsContainer
    {
        public ButterLibSettingsContainer(ILogger<ButterLibSettingsContainer> logger)
        {
            var minLogLevelProp = new StorageRef<Dropdown<string>>(new Dropdown<string>(new[]
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
                    builder.AddDropdown("MinLogLevel", "{=ButterLibSettings_Name_LogLevel}Log Level", 0, minLogLevelProp, dBuilder =>
                        dBuilder.SetOrder(1).SetRequireRestart(true).SetHintText("{=ButterLibSettings_Name_LogLevelDesc}Level of logs to write.")))
                .AddButterLibSubSystems()
                .BuildAsGlobal();
            RegisterSettings(settings);
        }
    }
}