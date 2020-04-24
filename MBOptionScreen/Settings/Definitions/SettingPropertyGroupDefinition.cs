using MBOptionScreen.ExtensionMethods;
using MBOptionScreen.Utils;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Localization;

namespace MBOptionScreen.Settings
{
    public class SettingPropertyGroupDefinition
    {
        public const string DefaultGroupName = "Misc";

        protected readonly TextObject _groupName;
        protected readonly TextObject _groupNameOverride;
        protected readonly List<SettingPropertyGroupDefinition> subGroups = new List<SettingPropertyGroupDefinition>();
        protected readonly List<SettingPropertyDefinition> settingProperties = new List<SettingPropertyDefinition>();

        public string GroupName { get; }
        public TextObject DisplayGroupName => _groupNameOverride.Length > 0 ? _groupNameOverride : _groupName;
        public int Order { get; }
        public IOrderedEnumerable<SettingPropertyGroupDefinition> SubGroups
        {
            get
            {
                return subGroups
                    .OrderByDescending(x => x.GroupName == SettingPropertyGroupDefinition.DefaultGroupName)
                    .ThenByDescending(x => x.Order)
                    .ThenByDescending(x => x, new AlphanumComparatorFast());
            }
        }
        public IOrderedEnumerable<SettingPropertyDefinition> SettingProperties
        {
            get
            {
                return settingProperties
                    .OrderBy(x => x.Order)
                    .ThenBy(x => x, new AlphanumComparatorFast());
            }
        }

        public SettingPropertyGroupDefinition(string groupName, string groupNameOverride = "", int order = -1)
        {
            _groupName = new TextObject(groupName, null);
            _groupNameOverride = new TextObject(groupNameOverride ?? "", null);
            GroupName = string.IsNullOrWhiteSpace(groupNameOverride) ? groupName : groupNameOverride!;
            Order = order;
        }

        public void Add(SettingPropertyDefinition settingProp)
        {
            settingProperties.Add(settingProp);
        }
        public void Add(SettingPropertyGroupDefinition settingProp)
        {
            subGroups.Add(settingProp);
        }

        public SettingPropertyGroupDefinition? GetGroup(string groupName) => subGroups.Find(x => x.GroupName == groupName);
        public SettingPropertyGroupDefinition? GetGroupFor(string groupName) => subGroups.GetGroup(groupName);

        public override string ToString() => $"{GroupName}";
    }
}