using HarmonyLib;

using MCM.Utils;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class SettingsPropertyGroupDefinitionWrapper : SettingsPropertyGroupDefinition
    {
        private static string? GetGroupName(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(GroupName));
            return propInfo?.GetValue(@object) as string;
        }
        private static string? GetGroupNameOverride(object @object)
        {
            var propInfo = @object.GetType().GetProperty("GroupNameOverride");
            return propInfo?.GetValue(@object) as string;
        }

        public SettingsPropertyGroupDefinitionWrapper(object @object) : base(
            GetGroupName(@object) ?? "ERROR",
            GetGroupNameOverride(@object) ?? "")
        {
            subGroups.AddRange(GetSubGroups(@object)
                .OrderByDescending(x => x.GroupName == DefaultGroupName)
                .ThenByDescending(x => x.Order)
                .ThenByDescending(x => x.DisplayGroupName.ToString(), new AlphanumComparatorFast()));
            settingProperties.AddRange(GetSettingProperties(@object)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.DisplayName.ToString(), new AlphanumComparatorFast()));
        }

        private IEnumerable<SettingsPropertyGroupDefinition> GetSubGroups(object @object)
        {
            var subGroupsProperty = AccessTools.Property(@object.GetType(), nameof(SubGroups)) ??
                                    AccessTools.Property(@object.GetType(), "SettingPropertyGroups");
            if (subGroupsProperty?.GetValue(@object) is IEnumerable list)
                foreach (var obj in list)
                    yield return new SettingsPropertyGroupDefinitionWrapper(obj);
        }
        private IEnumerable<SettingsPropertyDefinition> GetSettingProperties(object @object)
        {
            var settingPropertiesProperty = AccessTools.Property(@object.GetType(), nameof(SettingProperties));
            if (settingPropertiesProperty?.GetValue(@object) is IEnumerable list)
                foreach (var obj in list)
                    yield return new SettingsPropertyDefinitionWrapper(obj);
        }
    }
}