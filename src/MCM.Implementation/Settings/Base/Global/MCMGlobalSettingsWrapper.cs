using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;
using MCM.Abstractions.Settings.Properties;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Implementation.Settings.Base.Global
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
    [Version("e1.4.1",  1)]
    public class MCMGlobalSettingsWrapper : BaseMCMGlobalSettingsWrapper
    {
        private PropertyInfo? IdProperty { get; }
        private PropertyInfo? ModuleFolderNameProperty { get; }
        private PropertyInfo? DisplayNameProperty { get; }
        private PropertyInfo? UIVersionProperty { get; }
        private PropertyInfo? SubFolderProperty { get; }
        private PropertyInfo? SubGroupDelimiterProperty { get; }
        private PropertyInfo? FormatProperty { get; }
        private PropertyInfo? DiscovererProperty { get; }
        private MethodInfo? CreateNewMethod { get; }
        private MethodInfo? CopyAsNewMethod { get; }
        private MethodInfo? GetAvailablePresetsMethod { get; }
        private MethodInfo? GetSettingPropertyGroupsMethod { get; }
        private MethodInfo? GetUnsortedSettingPropertyGroupsMethod { get; }
        private MethodInfo? OnPropertyChangedMethod { get; }

        public override string Id => IdProperty?.GetValue(Object) as string ?? "ERROR";
        public override string FolderName => ModuleFolderNameProperty?.GetValue(Object) as string ?? string.Empty;
        public override string DisplayName => DisplayNameProperty?.GetValue(Object) as string ?? "ERROR";
        public override int UIVersion => UIVersionProperty?.GetValue(Object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? string.Empty;
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(Object) as char? ?? '/';
        public override string Format => FormatProperty?.GetValue(Object) as string ?? "json";
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }
        protected override ISettingsPropertyDiscoverer? Discoverer => DiscovererProperty?.GetValue(Object) is { } obj
            ? BaseSettingsPropertyDiscovererWrapper.Create(obj)
            : null;

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
            DiscovererProperty = AccessTools.Property(type, nameof(Discoverer));
            CreateNewMethod = AccessTools.Method(type, nameof(CreateNew));
            CopyAsNewMethod = AccessTools.Method(type, nameof(CopyAsNew));
            GetAvailablePresetsMethod = AccessTools.Method(type, nameof(GetAvailablePresets));
            GetSettingPropertyGroupsMethod = AccessTools.Method(type, nameof(GetSettingPropertyGroups));
            GetUnsortedSettingPropertyGroupsMethod = AccessTools.Method(type, nameof(GetUnsortedSettingPropertyGroups));
            OnPropertyChangedMethod = AccessTools.Method(type, nameof(OnPropertyChanged));

            IsCorrect = IdProperty != null && ModuleFolderNameProperty != null &&
                        DisplayNameProperty != null && UIVersionProperty != null &&
                        SubFolderProperty != null && SubGroupDelimiterProperty != null &&
                        FormatProperty != null && GetSettingPropertyGroupsMethod != null &&
                        OnPropertyChangedMethod != null && CreateNewMethod != null &&
                        GetAvailablePresetsMethod != null && CopyAsNewMethod != null &&
                        DisplayNameProperty != null;
        }

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName! });

        protected override BaseSettings CreateNew() => new MCMGlobalSettingsWrapper(CreateNewMethod!.Invoke(Object, Array.Empty<object>()));
        protected override BaseSettings CopyAsNew() => new MCMGlobalSettingsWrapper(CopyAsNewMethod!.Invoke(Object, Array.Empty<object>()));
        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
        {
            if (!(GetAvailablePresetsMethod?.Invoke(Object, Array.Empty<object>()) is IDictionary dict))
                return new Dictionary<string, Func<BaseSettings>>();

            var returnDict = new Dictionary<string, Func<BaseSettings>>();
            foreach (DictionaryEntry pair in dict)
                returnDict.Add((string) pair.Key, () => new MCMGlobalSettingsWrapper(((Func<object>) pair.Value).Invoke()));
            return returnDict;
        }


        public override List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups()
        {
            if (GetSettingPropertyGroupsMethod == null ||
                // Performance optimization. Do not use the default implementation.
                (GetSettingPropertyGroupsMethod.DeclaringType == typeof(BaseSettings) &&
                GetUnsortedSettingPropertyGroupsMethod?.DeclaringType?.IsGenericType == true &&
                GetUnsortedSettingPropertyGroupsMethod.DeclaringType.GetGenericTypeDefinition() == typeof(AttributeGlobalSettings<>)))
                return base.GetSettingPropertyGroups();

            return ((IEnumerable<object>) GetSettingPropertyGroupsMethod.Invoke(Object, Array.Empty<object>()) ?? new List<object>())
                .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
                .Cast<SettingsPropertyGroupDefinition>()
                .ToList();
        }
    }
}