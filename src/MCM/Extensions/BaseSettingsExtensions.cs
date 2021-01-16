using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Extensions
{
    public static class BaseSettingsExtensions
    {
        public static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(this BaseSettings settings) =>
            Enumerable.Empty<ISettingsPropertyDefinition>();
        public static IEnumerable<SettingsPropertyGroupDefinition> GetAllSettingPropertyGroupDefinitions(this BaseSettings settings) =>
            Enumerable.Empty<SettingsPropertyGroupDefinition>();
    }
}