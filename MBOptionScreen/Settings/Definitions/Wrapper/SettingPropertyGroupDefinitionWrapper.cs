using HarmonyLib;

using MBOptionScreen.Utils;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.Settings
{
    internal sealed class SettingPropertyGroupDefinitionWrapper : SettingPropertyGroupDefinition
    {
        private static string? GetGroupName(object @object)
        {
            var propInfo = @object.GetType().GetProperty("GroupName");
            return propInfo?.GetValue(@object) as string;
        }
        private static string? GetGroupNameOverride(object @object)
        {
            var propInfo = @object.GetType().GetProperty("GroupNameOverride");
            return propInfo?.GetValue(@object) as string;
        }

        public SettingPropertyGroupDefinitionWrapper(object @object) : base(
            GetGroupName(@object) ?? "ERROR",
            GetGroupNameOverride(@object) ?? "")
        {
            subGroups.AddRange(GetSubGroups(@object)
                .OrderByDescending(x => x.GroupName == SettingPropertyGroupDefinition.DefaultGroupName)
                .ThenByDescending(x => x.Order)
                .ThenByDescending(x => x, new AlphanumComparatorFast()));
            settingProperties.AddRange(GetSettingProperties(@object)
                .OrderBy(x => x.Order)
                .ThenBy(x => x, new AlphanumComparatorFast()));
        }

        private IEnumerable<SettingPropertyGroupDefinition> GetSubGroups(object @object)
        {
            var subGroupsProperty = AccessTools.Property(@object.GetType(), "SubGroups")
                ?? AccessTools.Property(@object.GetType(), "SettingPropertyGroups");
            var list = (IList) subGroupsProperty.GetValue(@object);
            foreach (var obj in list)
            {
                yield return new SettingPropertyGroupDefinitionWrapper(obj);
            }
        }
        private IEnumerable<SettingPropertyDefinition> GetSettingProperties(object @object)
        {
            var settingPropertiesProperty = AccessTools.Property(@object.GetType(), "SettingProperties");
            var list = (IList)settingPropertiesProperty.GetValue(@object);
            foreach (var obj in list)
            {
                yield return new SettingPropertyDefinitionWrapper(obj);
            }
        }
    }
}