using HarmonyLib;

using MCM.Extensions;

using System.Collections;
using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Models.Wrapper
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
        private static int? GetGroupOrder(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(Order));
            return propInfo?.GetValue(@object) as int?;
        }

        public SettingsPropertyGroupDefinitionWrapper(object @object) : base(
            GetGroupName(@object) ?? "ERROR",
            GetGroupNameOverride(@object) ?? "",
            GetGroupOrder(@object) ?? -1)
        {
            subGroups.AddRange(GetSubGroups(@object).SortDefault());
            settingProperties.AddRange(GetSettingProperties(@object).SortDefault());
        }

        private static IEnumerable<SettingsPropertyGroupDefinition> GetSubGroups(object @object)
        {
            var subGroupsProperty = AccessTools.Property(@object.GetType(), nameof(SubGroups)) ??
                                    AccessTools.Property(@object.GetType(), "SettingPropertyGroups");
            if (subGroupsProperty?.GetValue(@object) is IEnumerable list)
                foreach (var obj in list)
                    yield return new SettingsPropertyGroupDefinitionWrapper(obj);
        }
        private static IEnumerable<ISettingsPropertyDefinition> GetSettingProperties(object @object)
        {
            var settingPropertiesProperty = AccessTools.Property(@object.GetType(), nameof(SettingProperties));
            if (settingPropertiesProperty?.GetValue(@object) is IEnumerable list)
                foreach (var obj in list)
                    yield return new SettingsPropertyDefinitionWrapper(obj);
        }
    }
}