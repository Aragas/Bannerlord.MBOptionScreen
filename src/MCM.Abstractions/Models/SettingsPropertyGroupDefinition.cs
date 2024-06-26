﻿using MCM.Common;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class SettingsPropertyGroupDefinition
    {
        private class DefaultPropertyGroupDefinition : IPropertyGroupDefinition
        {
            public string GroupName => DefaultGroupName;
            public int GroupOrder => 0;
        }

        /// <summary>
        /// The default group used for settings that don't have a group explicitly set.
        /// </summary>
        public static readonly string DefaultGroupName = "{=SettingsPropertyGroupDefinition_Misc}Misc";

        /// <summary>
        /// The default group used for settings that don't have a group explicitly set.
        /// </summary>
        public static readonly IPropertyGroupDefinition DefaultGroup = new DefaultPropertyGroupDefinition();

        protected readonly string _groupNameRaw;
        protected readonly string _groupNameOverrideRaw = string.Empty;
        protected readonly List<SettingsPropertyGroupDefinition> subGroups = [];
        protected readonly List<ISettingsPropertyDefinition> settingProperties = [];

        protected char SubGroupDelimiter { get; set; }
        public SettingsPropertyGroupDefinition? Parent { get; set; }
        public string GroupNameRaw => _groupNameRaw;
        public string GroupName => DisplayGroupNameRaw;
        public string DisplayGroupNameRaw
        {
            get
            {
                if (Parent is null) return LocalizationUtils.Localize(_groupNameRaw);

                var localizedParentGroup = LocalizationUtils.Localize(Parent._groupNameRaw);
                var localizedGroupName = LocalizationUtils.Localize(_groupNameRaw);
                return localizedGroupName.Replace(localizedParentGroup, string.Empty).TrimStart(SubGroupDelimiter);
            }
        }

        public int Order { get; private set; }
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups => subGroups.SortDefault();
        public IEnumerable<ISettingsPropertyDefinition> SettingProperties => settingProperties.SortDefault();

        public bool IsEmpty => !SubGroups.Any() && SettingProperties.All(x => x.SettingType == SettingType.NONE);

        public SettingsPropertyGroupDefinition(string groupName, int order = -1)
        {
            _groupNameRaw = groupName;
            Order = order;
        }

        public SettingsPropertyGroupDefinition SetParent(SettingsPropertyGroupDefinition parent)
        {
            Parent = parent;
            return this;
        }

        public SettingsPropertyGroupDefinition SetSubGroupDelimiter(char subGroupDelimiter)
        {
            SubGroupDelimiter = subGroupDelimiter;
            return this;
        }

        public void Add(ISettingsPropertyDefinition settingProp)
        {
            settingProperties.Add(settingProp);
        }
        public void Add(SettingsPropertyGroupDefinition settingProp)
        {
            subGroups.Add(settingProp.SetParent(this));
        }

        public SettingsPropertyGroupDefinition? GetGroup(string groupName) => subGroups.GetGroupFromName(groupName);

        /// <inheritdoc/>
        public override string ToString() => GroupName;

        public SettingsPropertyGroupDefinition Clone(bool keepRefs = true)
        {
            var settings = new SettingsPropertyGroupDefinition(GroupName, order: Order).SetSubGroupDelimiter(SubGroupDelimiter);
            foreach (var prop in SettingProperties)
                settings.Add(prop.Clone(keepRefs));
            foreach (var subGroup in SubGroups)
                settings.Add(subGroup.Clone(keepRefs));
            return settings;
        }
    }
}