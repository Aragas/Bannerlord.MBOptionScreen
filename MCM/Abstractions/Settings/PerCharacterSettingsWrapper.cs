using HarmonyLib;

using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Settings
{
    public sealed class PerCharacterSettingsWrapper : BasePerCharacterSettingsWrapper
    {
        private PropertyInfo? IdProperty { get; }
        private PropertyInfo? ModuleFolderNameProperty { get; }
        private PropertyInfo? DisplayNameProperty { get; }
        private PropertyInfo? UIVersionProperty { get; }
        private PropertyInfo? SubFolderProperty { get; }
        private PropertyInfo? SubGroupDelimiterProperty { get; }
        private PropertyInfo? FormatProperty { get; }
        private MethodInfo? GetSettingPropertyGroupsMethod { get; }
        private MethodInfo? OnPropertyChangedMethod { get; }

        public override string Id => IdProperty?.GetValue(Object) as string ?? "ERROR";
        public override string FolderName => ModuleFolderNameProperty?.GetValue(Object) as string ?? "";
        public override string DisplayName => DisplayNameProperty?.GetValue(Object) as string ?? "ERROR";
        public override int UIVersion => UIVersionProperty?.GetValue(Object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? "";
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(Object) as char? ?? '/';
        public override string Format => FormatProperty?.GetValue(Object) as string ?? "json";
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        public PerCharacterSettingsWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();
            IdProperty = AccessTools.Property(type, nameof(Id));
            ModuleFolderNameProperty = AccessTools.Property(type, nameof(FolderName));
            DisplayNameProperty = AccessTools.Property(type, nameof(DisplayName));
            UIVersionProperty = AccessTools.Property(type, nameof(UIVersion));
            SubFolderProperty = AccessTools.Property(type, nameof(SubFolder));
            SubGroupDelimiterProperty = AccessTools.Property(type, nameof(SubGroupDelimiter));
            FormatProperty = AccessTools.Property(type, nameof(Format));
            GetSettingPropertyGroupsMethod = AccessTools.Method(type, nameof(GetSettingPropertyGroups));
            OnPropertyChangedMethod = AccessTools.Method(type, nameof(OnPropertyChanged));
        }

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName! });

        public override List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() => GetWrappedSettingPropertyGroups();
        private List<SettingsPropertyGroupDefinition> GetWrappedSettingPropertyGroups()
        {
            if (GetSettingPropertyGroupsMethod == null)
            {
                return GetUnsortedSettingPropertyGroups()
                    .OrderByDescending(x => x.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
                    .ThenByDescending(x => x.Order)
                    .ThenByDescending(x => x.DisplayGroupName.ToString(), new AlphanumComparatorFast())
                    .ToList();
            }

            return ((IEnumerable<object>)GetSettingPropertyGroupsMethod.Invoke(Object, Array.Empty<object>()))
                .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
                .Cast<SettingsPropertyGroupDefinition>()
                .ToList();
        }
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingProp in SettingsUtils.GetProperties(Object, Id))
            {
                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}