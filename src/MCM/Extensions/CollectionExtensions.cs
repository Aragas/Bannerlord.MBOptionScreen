using Bannerlord.ButterLib.Common.Helpers;

using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Extensions
{
    internal static class CollectionExtensions
    {
        public static IEnumerable<SettingsPropertyGroupDefinition> SortDefault(this IEnumerable<SettingsPropertyGroupDefinition> enumerable) => enumerable
            .OrderByDescending(x => x.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
            .ThenByDescending(x => x.Order)
            .ThenByDescending(x => x.GroupName, new AlphanumComparatorFast());

        public static IEnumerable<ISettingsPropertyDefinition> SortDefault(this IEnumerable<ISettingsPropertyDefinition> enumerable) => enumerable
            .OrderBy(x => x.Order)
            .ThenBy(x => x.DisplayName, new AlphanumComparatorFast());

        public static SettingsPropertyGroupDefinition? GetGroup(this IEnumerable<SettingsPropertyGroupDefinition> groupsList, string groupName) =>
            groupsList.FirstOrDefault(x => x.GroupName == groupName);
    }
}