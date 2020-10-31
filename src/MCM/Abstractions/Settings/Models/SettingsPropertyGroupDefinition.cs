using MCM.Abstractions.Settings.Definitions;
using MCM.Extensions;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Models
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
        public static readonly string DefaultGroupName = new TextObject("{=SettingsPropertyGroupDefinition_Misc}Misc").ToString();

        /// <summary>
        /// The default group used for settings that don't have a group explicitly set.
        /// </summary>
        public static readonly IPropertyGroupDefinition DefaultGroup = new DefaultPropertyGroupDefinition();

        protected readonly TextObject _groupName;
        protected readonly TextObject _groupNameOverride;
        protected readonly List<SettingsPropertyGroupDefinition> subGroups = new List<SettingsPropertyGroupDefinition>();
        protected readonly List<ISettingsPropertyDefinition> settingProperties = new List<ISettingsPropertyDefinition>();

        public string GroupName { get; }
        public TextObject DisplayGroupName => _groupNameOverride.Length > 0 ? _groupNameOverride : _groupName;
        public int Order { get; }
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups => subGroups.SortDefault();
        public IEnumerable<ISettingsPropertyDefinition> SettingProperties => settingProperties.SortDefault();

        public SettingsPropertyGroupDefinition(string groupName, string groupNameOverride = "", int order = -1)
        {
            _groupName = new TextObject(groupName);
            _groupNameOverride = new TextObject(groupNameOverride ?? string.Empty);
            GroupName = DisplayGroupName.ToString();
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
    }
}