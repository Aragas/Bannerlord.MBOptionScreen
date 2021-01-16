using Bannerlord.BUTR.Shared.Helpers;

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
        public IEnumerable<SettingsPropertyGroupDefinition> SubGroups => subGroups;
        public IEnumerable<ISettingsPropertyDefinition> SettingProperties => settingProperties;

        public SettingsPropertyGroupDefinition(string groupName, string groupNameOverride = "", int order = -1) { }

        public void Add(ISettingsPropertyDefinition settingProp) { }
        public void Add(SettingsPropertyGroupDefinition settingProp) { }

        public SettingsPropertyGroupDefinition? GetGroup(string groupName) => null;
        public SettingsPropertyGroupDefinition? GetGroupFor(string groupName) => null;

        public override string ToString() => GroupName;
    }
}