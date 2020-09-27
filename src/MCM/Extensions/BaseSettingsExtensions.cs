using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Extensions
{
    public static class BaseSettingsExtensions
    {
        public static List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups(this BaseSettings settings) => settings
            .GetUnsortedSettingPropertyGroups()
            .SortDefault()
            .ToList();

        public static IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups(this BaseSettings settings)
        {
            if (settings is IFluentSettings fluentSettings)
                return fluentSettings.SettingPropertyGroups;

            var discoverers = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IEnumerable<ISettingsPropertyDiscoverer>>() ?? Enumerable.Empty<ISettingsPropertyDiscoverer>();
            var discoverer = discoverers.FirstOrDefault(x => x.DiscoveryTypes.Any(y => y == settings.DiscoveryType));
            return SettingsUtils.GetSettingsPropertyGroups(settings.SubGroupDelimiter, discoverer?.GetProperties(settings) ?? Enumerable.Empty<ISettingsPropertyDefinition>());
        }

        public static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(this BaseSettings settings) =>
            settings.GetSettingPropertyGroups().SelectMany(SettingsUtils.GetAllSettingPropertyDefinitions);
        public static IEnumerable<SettingsPropertyGroupDefinition> GetAllSettingPropertyGroupDefinitions(this BaseSettings settings) =>
            settings.GetSettingPropertyGroups().SelectMany(SettingsUtils.GetAllSettingPropertyGroupDefinitions);
    }
}