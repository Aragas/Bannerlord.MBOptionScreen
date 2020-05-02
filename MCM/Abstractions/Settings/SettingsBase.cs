using MCM.Abstractions.ExtensionMethods;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.SettingsProvider;
using MCM.Utils;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Settings
{
    public abstract class SettingsBase<T> : SettingsBase where T : SettingsBase, new()
    {
        public static T? Instance => SettingsUtils.UnwrapSettings(BaseSettingsProvider.Instance.GetSettings(new T().Id)) as T;
    }

    public abstract class SettingsBase : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler? PropertyChanged;
        public abstract string Id { get; }
        public abstract string ModuleFolderName { get; }
        public abstract string ModName { get; }
        public virtual int UIVersion => 1;
        public virtual string SubFolder => "";
        protected virtual char SubGroupDelimiter => '/';
        public virtual string Format => "json";

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() => GetUnsortedSettingPropertyGroups()
            .OrderByDescending(x => x.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
            .ThenByDescending(x => x.Order)
            .ThenByDescending(x => x, new AlphanumComparatorFast())
            .ToList();
        protected abstract IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups();
        protected SettingsPropertyGroupDefinition GetGroupFor(SettingsPropertyDefinition sp, ICollection<SettingsPropertyGroupDefinition> rootCollection)
        {
            SettingsPropertyGroupDefinition? group;
            //Check if the intended group is a sub group
            if (sp.GroupName.Contains(SubGroupDelimiter))
            {
                //Intended group is a sub group. Must find it. First get the top group.
                var topGroupName = GetTopGroupName(sp.GroupName, out var truncatedGroupName);
                var topGroup = rootCollection.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingsPropertyGroupDefinition(sp.GroupName, topGroupName);
                    rootCollection.Add(topGroup);
                }
                //Find the sub group
                group = GetGroupForRecursive(truncatedGroupName, topGroup, sp);
            }
            else
            {
                //Group is not a subgroup, can find it in the main list of groups.
                group = rootCollection.GetGroup(sp.GroupName);
                if (group == null)
                {
                    group = new SettingsPropertyGroupDefinition(sp.GroupName, order: sp.GroupOrder);
                    rootCollection.Add(group);
                }
            }
            return group;
        }
        protected SettingsPropertyGroupDefinition GetGroupForRecursive(string groupName, SettingsPropertyGroupDefinition sgp, SettingsPropertyDefinition sp)
        {
            if (groupName.Contains(SubGroupDelimiter))
            {
                //Need to go deeper
                var topGroupName = GetTopGroupName(groupName, out var truncatedGroupName);
                var topGroup = sgp.GetGroupFor(topGroupName);
                if (topGroup == null)
                {
                    // Order will not be passed to the subgroup
                    topGroup = new SettingsPropertyGroupDefinition(sp.GroupName, topGroupName);
                    sgp.Add(topGroup);
                }
                return GetGroupForRecursive(truncatedGroupName, topGroup, sp);
            }
            else
            {
                //Reached the bottom level, can return the final group.
                var group = sgp.GetGroup(groupName);
                if (group == null)
                {
                    group = new SettingsPropertyGroupDefinition(sp.GroupName, groupName, sp.GroupOrder);
                    sgp.Add(group);
                }
                return group;
            }
        }
        protected string GetTopGroupName(string groupName, out string truncatedGroupName)
        {
            var index = groupName.IndexOf(SubGroupDelimiter);
            var topGroupName = groupName.Substring(0, index);

            truncatedGroupName = groupName.Remove(0, index + 1);
            return topGroupName;
        }
    }
}