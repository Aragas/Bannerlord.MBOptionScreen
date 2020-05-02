using MCM.Abstractions.ExtensionMethods;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Definitions
{
    public class SettingsPropertyGroupDefinition
    {
        public const string DefaultGroupName = "Misc";

        protected readonly TextObject _groupName;
        protected readonly TextObject _groupNameOverride;
        protected readonly List<SettingsPropertyGroupDefinition> subGroups = new List<SettingsPropertyGroupDefinition>();
        protected readonly List<SettingsPropertyDefinition> settingProperties = new List<SettingsPropertyDefinition>();

        public string GroupName { get; }
        public TextObject DisplayGroupName => _groupNameOverride.Length > 0 ? _groupNameOverride : _groupName;
        public int Order { get; }
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups
        {
            get
            {
                return subGroups
                    .OrderByDescending(x => x.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
                    .ThenByDescending(x => x.Order)
                    .ThenByDescending(x => x, new AlphanumComparatorFast());
            }
        }
        public IEnumerable<SettingsPropertyDefinition> SettingProperties
        {
            get
            {
                return settingProperties
                    .OrderBy(x => x.Order)
                    .ThenBy(x => x, new AlphanumComparatorFast());
            }
        }

        public SettingsPropertyGroupDefinition(string groupName, string groupNameOverride = "", int order = -1)
        {
            _groupName = new TextObject(groupName, null);
            _groupNameOverride = new TextObject(groupNameOverride ?? "", null);
            GroupName = string.IsNullOrWhiteSpace(groupNameOverride) ? groupName : groupNameOverride!;
            Order = order;
        }

        public void Add(SettingsPropertyDefinition settingProp)
        {
            settingProperties.Add(settingProp);
        }
        public void Add(SettingsPropertyGroupDefinition settingProp)
        {
            subGroups.Add(settingProp);
        }

        public SettingsPropertyGroupDefinition? GetGroup(string groupName) => subGroups.Find(x => x.GroupName == groupName);
        public SettingsPropertyGroupDefinition? GetGroupFor(string groupName) => subGroups.GetGroup(groupName);

        public override string ToString() => $"{GroupName}";
    }
}