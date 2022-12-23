using HarmonyLib.BUTR.Extensions;

using System.Collections;
using System.Collections.Generic;

namespace MCM.Abstractions.Wrapper
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class SettingsPropertyGroupDefinitionWrapper : SettingsPropertyGroupDefinition
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
        private static char? GetSubGroupDelimiter(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(SubGroupDelimiter));
            return propInfo?.GetValue(@object) as char?;
        }

        public SettingsPropertyGroupDefinitionWrapper(object @object) : base(GetGroupName(@object) ?? "ERROR", GetGroupOrder(@object) ?? -1)
        {
            SetSubGroupDelimiter(GetSubGroupDelimiter(@object) ?? '/');
            subGroups.AddRange(GetSubGroups(@object).SortDefault());
            settingProperties.AddRange(GetSettingProperties(@object).SortDefault());
        }

        private static IEnumerable<SettingsPropertyGroupDefinition> GetSubGroups(object @object)
        {
            var subGroupsProperty = AccessTools2.Property(@object.GetType(), nameof(SubGroups)) ??
                                    AccessTools2.Property(@object.GetType(), "SettingPropertyGroups");
            if (subGroupsProperty?.GetValue(@object) is IEnumerable list)
            {
                foreach (var obj in list)
                {
                    yield return new SettingsPropertyGroupDefinitionWrapper(obj);
                }
            }
        }
        private static IEnumerable<ISettingsPropertyDefinition> GetSettingProperties(object @object)
        {
            var settingPropertiesProperty = AccessTools2.Property(@object.GetType(), nameof(SettingProperties));
            if (settingPropertiesProperty?.GetValue(@object) is IEnumerable list)
            {
                foreach (var obj in list)
                {
                    yield return new SettingsPropertyDefinitionWrapper(obj);
                }
            }
        }
    }
}