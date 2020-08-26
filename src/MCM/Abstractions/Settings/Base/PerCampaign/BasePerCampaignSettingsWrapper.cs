using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;
using MCM.Utils;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public abstract class BasePerCampaignSettingsWrapper : PerCampaignSettings, IWrapper
    {
        private delegate string GetIdDelegate();
        private delegate string GetFolderNameDelegate();
        private delegate string GetDisplayNameDelegate();
        private delegate int GetUIVersionDelegate();
        private delegate string GetSubFolderDelegate();
        private delegate char GetSubGroupDelimiterDelegate();
        private delegate string GetFormatDelegate();
        private delegate List<SettingsPropertyGroupDefinition> GetSettingPropertyGroupsDelegate();
        private delegate void OnPropertyChangedDelegate(string? propertyName);

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly GetFolderNameDelegate? _getFolderNameDelegate;
        private readonly GetDisplayNameDelegate? _getDisplayNameDelegate;
        private readonly GetUIVersionDelegate? _getUIVersionDelegate;
        private readonly GetSubFolderDelegate? _getSubFolderDelegate;
        private readonly GetSubGroupDelimiterDelegate? _getSubGroupDelimiterDelegate;
        private readonly GetFormatDelegate? _getFormatDelegate;
        private readonly GetSettingPropertyGroupsDelegate? _methodGetSettingPropertyGroupsDelegate;
        private readonly OnPropertyChangedDelegate? _methodOnPropertyChangedDelegate;

        /// <inheritdoc/>
        public object Object { get; protected set; }

        /// <inheritdoc/>
        public override string Id => _getIdDelegate?.Invoke() ?? "ERROR";
        /// <inheritdoc/>
        public override string FolderName => _getFolderNameDelegate?.Invoke() ?? string.Empty;
        /// <inheritdoc/>
        public override string DisplayName => _getDisplayNameDelegate?.Invoke() ?? "ERROR";
        /// <inheritdoc/>
        public override int UIVersion => _getUIVersionDelegate?.Invoke() ?? 1;
        /// <inheritdoc/>
        public override string SubFolder => _getSubFolderDelegate?.Invoke() ?? string.Empty;
        /// <inheritdoc/>
        protected override char SubGroupDelimiter => _getSubGroupDelimiterDelegate?.Invoke() ?? '/';
        /// <inheritdoc/>
        public override string Format => _getFormatDelegate?.Invoke() ?? "json";
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

            _getIdDelegate = AccessTools2.GetDelegate<GetIdDelegate>(@object, AccessTools.Property(type, nameof(Id)).GetGetMethod());
            _getFolderNameDelegate = AccessTools2.GetDelegate<GetFolderNameDelegate>(@object, AccessTools.Property(type, nameof(FolderName)).GetGetMethod());
            _getDisplayNameDelegate = AccessTools2.GetDelegate<GetDisplayNameDelegate>(@object, AccessTools.Property(type, nameof(DisplayName)).GetGetMethod());
            _getUIVersionDelegate = AccessTools2.GetDelegate<GetUIVersionDelegate>(@object, AccessTools.Property(type, nameof(UIVersion)).GetGetMethod());
            _getSubFolderDelegate = AccessTools2.GetDelegate<GetSubFolderDelegate>(@object, AccessTools.Property(type, nameof(SubFolder)).GetGetMethod());
            _getSubGroupDelimiterDelegate = AccessTools2.GetDelegate<GetSubGroupDelimiterDelegate>(@object, AccessTools.Property(type, nameof(SubGroupDelimiter)).GetGetMethod());
            _getFormatDelegate = AccessTools2.GetDelegate<GetFormatDelegate>(@object, AccessTools.Property(type, nameof(Format)).GetGetMethod());
            _methodGetSettingPropertyGroupsDelegate = AccessTools2.GetDelegate<GetSettingPropertyGroupsDelegate>(@object, AccessTools.Method(type, nameof(GetSettingPropertyGroups)));
            _methodOnPropertyChangedDelegate = AccessTools2.GetDelegate<OnPropertyChangedDelegate>(@object, AccessTools.Method(type, nameof(OnPropertyChanged)));
        }

        /// <inheritdoc/>
        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            _methodOnPropertyChangedDelegate?.Invoke(propertyName);

        /// <inheritdoc/>
        public override List<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() =>
            (_methodGetSettingPropertyGroupsDelegate?.Invoke() ?? Enumerable.Empty<object>())
            .Select(o => new SettingsPropertyGroupDefinitionWrapper(o))
            .Cast<SettingsPropertyGroupDefinition>()
            .ToList();
        /// <inheritdoc/>
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() =>
            SettingsUtils.GetSettingsPropertyGroups(SubGroupDelimiter, Discoverer?.GetProperties(Object) ?? Enumerable.Empty<ISettingsPropertyDefinition>());
    }
}