using Bannerlord.BUTR.Shared.Helpers;

using MCM.Extensions;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Models
{
    public class SettingsPropertyGroupDefinition
    {
        public static readonly string DefaultGroupName = TextObjectHelper.Create("{=SettingsPropertyGroupDefinition_Misc}Misc").ToString();

        protected readonly TextObject _groupName;
        protected readonly TextObject _groupNameOverride;
        protected readonly List<SettingsPropertyGroupDefinition> subGroups = new();
        protected readonly List<ISettingsPropertyDefinition> settingProperties = new();

        public string GroupName { get; }
        public TextObject DisplayGroupName => _groupNameOverride.Length > 0 ? _groupNameOverride : _groupName;
        public int Order { get; }
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups => subGroups.SortDefault();
        public IEnumerable<ISettingsPropertyDefinition> SettingProperties => settingProperties.SortDefault();

        public SettingsPropertyGroupDefinition(string groupName, string groupNameOverride = "", int order = -1)
        {
            _groupName = TextObjectHelper.Create(groupName);
            _groupNameOverride = TextObjectHelper.Create(groupNameOverride ?? "");
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

        public override string ToString() => GroupName;
    }
}