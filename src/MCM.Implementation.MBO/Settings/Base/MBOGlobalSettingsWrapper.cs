extern alias v4;

using HarmonyLib;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

using v4::MCM.Abstractions.Settings.Base;

namespace MCM.Implementation.MBO.Settings.Base
{
    public class MBOGlobalSettingsWrapper : BaseMBOGlobalSettingsWrapper
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
        public override string FolderName => ModuleFolderNameProperty?.GetValue(Object) as string ?? string.Empty;
        public override string DisplayName => ModNameProperty?.GetValue(Object) as string ?? "ERROR";
        public override int UIVersion => UIVersionProperty?.GetValue(Object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? string.Empty;
        public override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(Object) as char? ?? '/';
        public override string FormatType => FormatProperty?.GetValue(Object) as string ?? "json";
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
        }

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName! });

        protected override BaseSettings CreateNew() => new MBOGlobalSettingsWrapper(Activator.CreateInstance(Object.GetType()));
    }
}