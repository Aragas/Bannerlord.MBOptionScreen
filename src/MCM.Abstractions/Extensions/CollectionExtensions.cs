using Bannerlord.ModuleManager;

using MCM.Common;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions
{
    internal static class CollectionExtensions
    {
        public static IOrderedEnumerable<SettingsPropertyGroupDefinition> SortDefault(this IEnumerable<SettingsPropertyGroupDefinition> enumerable) => enumerable
            .OrderByDescending(x => x.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
            .ThenByDescending(x => x.Order)
            .ThenByDescending(x => LocalizationUtils.Localize(x.GroupName), new AlphanumComparatorFast());

        public static IOrderedEnumerable<ISettingsPropertyDefinition> SortDefault(this IEnumerable<ISettingsPropertyDefinition> enumerable) => enumerable
            .OrderBy(x => x.Order)
            .ThenBy(x => LocalizationUtils.Localize(x.DisplayName), new AlphanumComparatorFast());

        public static SettingsPropertyGroupDefinition? GetGroupFromName(this IEnumerable<SettingsPropertyGroupDefinition> groupsList, string groupName) =>
            groupsList.FirstOrDefault(x => x.GroupNameRaw == groupName);
    }
}