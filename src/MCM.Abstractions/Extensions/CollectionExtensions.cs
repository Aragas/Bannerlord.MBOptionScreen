using Bannerlord.ModuleManager;

using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Extensions
{
    internal static class CollectionExtensions
    {
        public static T? GetRandomElementWithPredicate<T>(this IReadOnlyCollection<T> collection, Func<T, bool> predicate)
        {
            if (collection.Count == 0)
                return default;

            var predicateMatchCount = collection.Count(predicate);
            if (predicateMatchCount == 0)
                return default;

            var randomInt = MBRandom.RandomInt(predicateMatchCount);
            foreach (var element in collection)
            {
                if (predicate(element))
                {
                    randomInt--;
                    if (randomInt <= 0)
                    {
                        return element;
                    }
                }
            }

            return default;
        }

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