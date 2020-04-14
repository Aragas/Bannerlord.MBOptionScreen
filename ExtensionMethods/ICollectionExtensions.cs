using System.Collections.Generic;
using System.Linq;

namespace ModLib
{
    public static class ICollectionExtensions
    {
        public static SettingPropertyGroupDefinition GetGroup(this ICollection<SettingPropertyGroupDefinition> groupsList, string groupName)
        {
            return groupsList.Where((x) => x.GroupName == groupName).FirstOrDefault();
        }
    }
}
