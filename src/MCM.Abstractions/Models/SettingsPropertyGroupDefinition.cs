using MCM.Common;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions
{
    public class SettingsPropertyGroupDefinition
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
        protected readonly List<SettingsPropertyGroupDefinition> subGroups = new();
        protected readonly List<ISettingsPropertyDefinition> settingProperties = new();

        protected char SubGroupDelimiter { get; set; }
        public SettingsPropertyGroupDefinition? Parent { get; set; }
        public string GroupName => DisplayGroupNameRaw;
        public string DisplayGroupNameRaw
        {
            get
            {
                if (Parent is null) return _groupNameRaw;

                var localizedParentGroup = LocalizationUtils.Localize(Parent._groupNameRaw);
                var localizedGroupName = LocalizationUtils.Localize(_groupNameRaw);
                return localizedGroupName.Replace(localizedParentGroup, string.Empty).TrimStart(SubGroupDelimiter);
            }
        }

        public int Order { get; }
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups => subGroups.SortDefault();
        public IEnumerable<ISettingsPropertyDefinition> SettingProperties => settingProperties.SortDefault();

        public SettingsPropertyGroupDefinition(string groupName, int order = -1)
        {
            _groupNameRaw = groupName;
            Order = order;
        }

        [Obsolete("Override not needed", true)]
        public SettingsPropertyGroupDefinition(string groupName, string? _, int order = -1)
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

        public SettingsPropertyGroupDefinition? GetGroup(string groupName) => subGroups.Find(x => x.GroupName == groupName);
        public SettingsPropertyGroupDefinition? GetGroupFor(string groupName) => subGroups.GetGroupFromLocalizedName(groupName);

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