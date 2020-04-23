using MBOptionScreen.Settings;

using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.ExtensionMethods
{
    public static class ICollectionExtensions
    {
        public static SettingPropertyGroupDefinition? GetGroup(this ICollection<SettingPropertyGroupDefinition> groupsList, string groupName)
        {
            return groupsList.FirstOrDefault(x => x.GroupName == groupName);
        }
    }
}