using ModLib.GUI.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ModLib
{
    public static class ICollectionExtensions
    {
        public static SettingPropertyGroup GetGroup(this ICollection<SettingPropertyGroup> groupsList, string groupName)
        {
            return groupsList.Where((x) => x.GroupName == groupName).FirstOrDefault();
        }
    }
}
