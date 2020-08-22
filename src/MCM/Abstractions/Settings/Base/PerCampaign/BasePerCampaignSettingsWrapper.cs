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

namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public abstract class BasePerCampaignSettingsWrapper : PerCampaignSettings, IDependencyBase, IWrapper
    {
        public static BasePerCampaignSettingsWrapper Create(object @object) =>
            DI.GetBaseImplementations<BasePerCampaignSettingsWrapper, PerCampaignSettingsWrapper>(@object)
            .FirstOrDefault(w => w.IsCorrect);

        /// <inheritdoc/>
        public object Object { get; protected set; }
        private PropertyInfo? IdProperty { get; }
        private PropertyInfo? ModuleFolderNameProperty { get; }
        private PropertyInfo? DisplayNameProperty { get; }
        private PropertyInfo? UIVersionProperty { get; }
        private PropertyInfo? SubFolderProperty { get; }
        private PropertyInfo? SubGroupDelimiterProperty { get; }
        private PropertyInfo? FormatProperty { get; }
        private MethodInfo? GetSettingPropertyGroupsMethod { get; }
        private MethodInfo? OnPropertyChangedMethod { get; }
        /// <inheritdoc/>
        public bool IsCorrect { get; protected set; }

        /// <inheritdoc/>
        public override string Id => IdProperty?.GetValue(Object) as string ?? "ERROR";
        /// <inheritdoc/>
        public override string FolderName => ModuleFolderNameProperty?.GetValue(Object) as string ?? string.Empty;
        /// <inheritdoc/>
        public override string DisplayName => DisplayNameProperty?.GetValue(Object) as string ?? "ERROR";
        /// <inheritdoc/>
        public override int UIVersion => UIVersionProperty?.GetValue(Object) as int? ?? 1;
        /// <inheritdoc/>
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? string.Empty;
        /// <inheritdoc/>
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(Object) as char? ?? '/';
        /// <inheritdoc/>
        public override string Format => FormatProperty?.GetValue(Object) as string ?? "json";
        /// <inheritdoc/>
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        protected BasePerCampaignSettingsWrapper(object @object)
        {
            Object = @object;
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

        /// <inheritdoc/>
        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName! });

        /// <inheritdoc/>
        protected override BaseSettings CreateNew()
        {
            var newObject = Activator.CreateInstance(Object.GetType());
            return Create(newObject);
        }

        /// <inheritdoc/>
        public override List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() =>
            ((IEnumerable<object>) (GetSettingPropertyGroupsMethod?.Invoke(Object, Array.Empty<object>()) ?? Array.Empty<object>()))
            .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
            .Cast<SettingsPropertyGroupDefinition>()
            .ToList();
        /// <inheritdoc/>
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() =>
            SettingsUtils.GetSettingsPropertyGroups(SubGroupDelimiter, Discoverer?.GetProperties(Object) ?? Array.Empty<ISettingsPropertyDefinition>());
    }
}