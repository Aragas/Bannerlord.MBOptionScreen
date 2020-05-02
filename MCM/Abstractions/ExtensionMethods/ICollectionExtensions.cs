using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.ExtensionMethods
{
    internal static class ICollectionExtensions
    {
        public static SettingsPropertyGroupDefinition? GetGroup(this ICollection<SettingsPropertyGroupDefinition> groupsList, string groupName)
        {
            return groupsList.FirstOrDefault(x => x.GroupName == groupName);
        }
    }
}