using MCM.Common;

using System.Collections.Generic;

namespace MCM.Abstractions
{
    public class SettingsPropertyGroupDefinition
    {
        private class DefaultPropertyGroupDefinition : IPropertyGroupDefinition
        {
            public string GroupName { get; } = DefaultGroupName;
            public int GroupOrder { get; } = 0;
        }

        /// <summary>
        /// The default group used for settings that don't have a group explicitly set.
        /// </summary>
        public static readonly string DefaultGroupName = LocalizationUtils.Localize("{=SettingsPropertyGroupDefinition_Misc}Misc");

        /// <summary>
        /// The default group used for settings that don't have a group explicitly set.
        /// </summary>
        public static readonly IPropertyGroupDefinition DefaultGroup = new DefaultPropertyGroupDefinition();

        protected readonly string _groupNameRaw;
        protected readonly string _groupNameOverrideRaw;
        protected readonly List<SettingsPropertyGroupDefinition> subGroups = new();
        protected readonly List<ISettingsPropertyDefinition> settingProperties = new();

        public string GroupName { get; }
        public string DisplayGroupNameRaw => _groupNameOverrideRaw.Length > 0 ? _groupNameOverrideRaw : _groupNameRaw;
        public int Order { get; }
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups => subGroups.SortDefault();
        public IEnumerable<ISettingsPropertyDefinition> SettingProperties => settingProperties.SortDefault();

        public SettingsPropertyGroupDefinition(string groupName, string? groupNameOverride = "", int order = -1)
        {
            _groupNameRaw = groupName;
            _groupNameOverrideRaw = groupNameOverride ?? string.Empty;
            GroupName = LocalizationUtils.Localize(DisplayGroupNameRaw);
            Order = order;
        }

        public void Add(ISettingsPropertyDefinition settingProp)
        {
            settingProperties.Add(settingProp);
        }
        public void Add(SettingsPropertyGroupDefinition settingProp)
        {
            subGroups.Add(settingProp);
        }

        public SettingsPropertyGroupDefinition? GetGroup(string groupName) => subGroups.Find(x => x.GroupName == groupName);
        public SettingsPropertyGroupDefinition? GetGroupFor(string groupName) => subGroups.GetGroup(groupName);

        /// <inheritdoc/>
        public override string ToString() => GroupName;

        public SettingsPropertyGroupDefinition Clone(bool keepRefs = true)
        {
            var settings = new SettingsPropertyGroupDefinition(GroupName, order: Order);
            foreach (var prop in SettingProperties)
                settings.Add(prop.Clone(keepRefs));
            foreach (var subGroup in SubGroups)
                settings.Add(subGroup.Clone(keepRefs));
            return settings;
        }
    }
}