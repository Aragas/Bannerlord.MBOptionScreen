using HarmonyLib;

using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MCM.Abstractions.Settings
{
    public class SettingsWrapper : SettingsBase, INotifyPropertyChanged
    {
        public readonly object Object;
        private PropertyInfo? IdProperty { get; }
        private PropertyInfo? ModuleFolderNameProperty { get; }
        private PropertyInfo? ModNameProperty { get; }
        private PropertyInfo? UIVersionProperty { get; }
        private PropertyInfo? SubFolderProperty { get; }
        private PropertyInfo? SubGroupDelimiterProperty { get; }
        private PropertyInfo? FormatProperty { get; }
        private MethodInfo? GetSettingPropertyGroupsMethod { get; }
        private MethodInfo? OnPropertyChangedMethod { get; }

        public override string Id => IdProperty?.GetValue(Object) as string ?? "ERROR";
        public override string ModuleFolderName // TODO: ModLib throws for some reason
        {
            get
            {
                try { return ModuleFolderNameProperty?.GetValue(Object) as string ?? ""; }
                catch (TargetInvocationException) { return ""; }
            }
        }
        public override string ModName => ModNameProperty?.GetValue(Object) as string ?? "ERROR";
        public override int UIVersion => UIVersionProperty?.GetValue(Object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? "";
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(Object) as char? ?? '/';
        public override string Format => FormatProperty?.GetValue(Object) as string ?? "json";
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                if (Object is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged += value;
            }
            remove
            {
                if (Object is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged -= value;
            }
        }

        public SettingsWrapper(object @object)
        {
            Object = @object;

            var type = @object.GetType();
            IdProperty = AccessTools.Property(type, nameof(Id)) ??
                         AccessTools.Property(type, "ID");
            ModuleFolderNameProperty = AccessTools.Property(type, nameof(ModuleFolderName));
            ModNameProperty = AccessTools.Property(type, nameof(ModName));
            UIVersionProperty = AccessTools.Property(type, nameof(UIVersion));
            SubFolderProperty = AccessTools.Property(type, nameof(SubFolder));
            SubGroupDelimiterProperty = AccessTools.Property(type, nameof(SubGroupDelimiter));
            FormatProperty = AccessTools.Property(type, nameof(Format));
            GetSettingPropertyGroupsMethod = AccessTools.Method(type, nameof(GetSettingPropertyGroups));
            OnPropertyChangedMethod = AccessTools.Method(type, nameof(OnPropertyChanged));
        }

        protected override void OnPropertyChanged(string? propertyName = null) =>
            OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName });

        public override List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() => GetWrappedSettingPropertyGroups();
        private List<SettingsPropertyGroupDefinition> GetWrappedSettingPropertyGroups()
        {
            if (GetSettingPropertyGroupsMethod == null)
            {
                return GetUnsortedSettingPropertyGroups()
                    .OrderByDescending(x => x.GroupName == SettingsPropertyGroupDefinition.DefaultGroupName)
                    .ThenByDescending(x => x.Order)
                    .ThenByDescending(x => x, new AlphanumComparatorFast())
                    .ToList();
            }
            else
            {
                return ((IEnumerable<object>) GetSettingPropertyGroupsMethod.Invoke(Object, Array.Empty<object>()))
                    .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
                    .Cast<SettingsPropertyGroupDefinition>()
                    .ToList();
            }
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