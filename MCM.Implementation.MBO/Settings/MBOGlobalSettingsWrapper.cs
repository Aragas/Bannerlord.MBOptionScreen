using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Implementation.MBO.Settings
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
    internal class MBOGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
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
        public override string ModuleFolderName => ModuleFolderNameProperty?.GetValue(Object) as string ?? "";
        public override string DisplayName => ModNameProperty?.GetValue(Object) as string ?? "ERROR";
        public override int UIVersion => UIVersionProperty?.GetValue(Object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? "";
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(Object) as char? ?? '/';
        public override string Format => FormatProperty?.GetValue(Object) as string ?? "json";
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        public MBOGlobalSettingsWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();
            IdProperty = AccessTools.Property(type, "Id");
            ModuleFolderNameProperty = AccessTools.Property(type, "ModuleFolderName");
            ModNameProperty = AccessTools.Property(type, "ModName");
            UIVersionProperty = AccessTools.Property(type, "UIVersion");
            SubFolderProperty = AccessTools.Property(type, "SubFolder");
            SubGroupDelimiterProperty = AccessTools.Property(type, "SubGroupDelimiter");
            FormatProperty = AccessTools.Property(type, "Format");
            GetSettingPropertyGroupsMethod = AccessTools.Method(type, "GetSettingPropertyGroups");
            OnPropertyChangedMethod = AccessTools.Method(type, "OnPropertyChanged");

            IsCorrect = IdProperty != null && ModuleFolderNameProperty != null &&
                        ModNameProperty != null && UIVersionProperty != null &&
                        SubFolderProperty != null && SubGroupDelimiterProperty != null &&
                        GetSettingPropertyGroupsMethod != null
                        // Not present in v1
                        /* FormatProperty != null &&
                        OnPropertyChangedMethod != null*/;
        }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName! });

        public override List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() =>
            ((IEnumerable<object>) GetSettingPropertyGroupsMethod!.Invoke(Object, Array.Empty<object>()))
            .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
            .Cast<SettingsPropertyGroupDefinition>()
            .ToList();
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => Array.Empty<SettingsPropertyGroupDefinition>();
    }
}