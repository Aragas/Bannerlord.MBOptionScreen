using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Implementation.Settings.Properties;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Implementation.Settings
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    public class MCMGlobalSettingsWrapper : BaseGlobalSettingsWrapper
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

        public MCMGlobalSettingsWrapper(object @object) : base(@object)
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

            IsCorrect = IdProperty != null && ModuleFolderNameProperty != null &&
                        DisplayNameProperty != null && UIVersionProperty != null &&
                        SubFolderProperty != null && SubGroupDelimiterProperty != null &&
                        FormatProperty != null && GetSettingPropertyGroupsMethod != null &&
                        OnPropertyChangedMethod != null;
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

            return ((IEnumerable<object>) GetSettingPropertyGroupsMethod.Invoke(Object, Array.Empty<object>()) ?? new List<object>())
                .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
                .Cast<SettingsPropertyGroupDefinition>()
                .ToList();
        }
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            var groups = new List<SettingsPropertyGroupDefinition>();
            foreach (var settingProp in new MCMSettingsPropertyDiscoverer().GetProperties(Object, Id))
            {
                var group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}