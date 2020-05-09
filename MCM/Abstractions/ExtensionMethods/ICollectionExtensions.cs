using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.ExtensionMethods
{
    internal static class ICollectionExtensions
    {
        public static SettingsPropertyGroupDefinition? GetGroup(this IEnumerable<SettingsPropertyGroupDefinition> groupsList, string groupName)
        {
            return groupsList.FirstOrDefault(x => x.GroupName == groupName);
        }
    }
}