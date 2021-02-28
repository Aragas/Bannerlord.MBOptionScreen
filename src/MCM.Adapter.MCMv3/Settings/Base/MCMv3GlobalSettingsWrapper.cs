extern alias v3;
extern alias v4;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using v4::MCM.Abstractions.Settings.Base;
using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Utils;

using MCMv3BaseSettings = v3::MCM.Abstractions.Settings.Base.BaseSettings;

namespace MCM.Adapter.MCMv3.Settings.Base
{
    internal sealed class MCMv3GlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        private delegate string GetIdDelegate();
        private delegate string GetFolderNameDelegate();
        private delegate string GetDisplayNameDelegate();
        private delegate int GetUIVersionDelegate();
        private delegate string GetSubFolderDelegate();
        private delegate char GetSubGroupDelimiterDelegate();
        private delegate string GetFormatDelegate();
        private delegate void OnPropertyChangedDelegate(string? propertyName);
        private delegate IDictionary<string, Func<MCMv3BaseSettings>> GetAvailablePresetsDelegate();

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly GetFolderNameDelegate? _getFolderNameDelegate;
        private readonly GetDisplayNameDelegate? _getDisplayNameDelegate;
        private readonly GetUIVersionDelegate? _getUIVersionDelegate;
        private readonly GetSubFolderDelegate? _getSubFolderDelegate;
        private readonly GetSubGroupDelimiterDelegate? _getSubGroupDelimiterDelegate;
        private readonly GetFormatDelegate? _getFormatDelegate;
        private readonly OnPropertyChangedDelegate? _methodOnPropertyChangedDelegate;
        private readonly GetAvailablePresetsDelegate? _methodGetAvailablePresetsDelegate;

        public override string Id => _getIdDelegate?.Invoke() ?? "ERROR";
        public override string FolderName => _getFolderNameDelegate?.Invoke() ?? string.Empty;
        public override string DisplayName => _getDisplayNameDelegate?.Invoke() ?? "ERROR";
        public override int UIVersion => _getUIVersionDelegate?.Invoke() ?? 1;
        public override string SubFolder => _getSubFolderDelegate?.Invoke() ?? string.Empty;
        public override char SubGroupDelimiter => _getSubGroupDelimiterDelegate?.Invoke() ?? '/';
        public override string FormatType => _getFormatDelegate?.Invoke() ?? "json";
        public override string DiscoveryType => "mcm_v3_attributes";
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        public MCMv3GlobalSettingsWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            _getIdDelegate = AccessTools3.GetDelegate<GetIdDelegate>(@object, AccessTools.Property(type, nameof(MCMv3BaseSettings.Id)).GetMethod);
            _getFolderNameDelegate = AccessTools3.GetDelegate<GetFolderNameDelegate>(@object, AccessTools.Property(type, nameof(MCMv3BaseSettings.FolderName)).GetMethod);
            _getDisplayNameDelegate = AccessTools3.GetDelegate<GetDisplayNameDelegate>(@object, AccessTools.Property(type, nameof(MCMv3BaseSettings.DisplayName)).GetMethod);
            _getUIVersionDelegate = AccessTools3.GetDelegate<GetUIVersionDelegate>(@object, AccessTools.Property(type, nameof(MCMv3BaseSettings.UIVersion)).GetMethod);
            _getSubFolderDelegate = AccessTools3.GetDelegate<GetSubFolderDelegate>(@object, AccessTools.Property(type, nameof(MCMv3BaseSettings.SubFolder)).GetMethod);
            _getSubGroupDelimiterDelegate = AccessTools3.GetDelegate<GetSubGroupDelimiterDelegate>(@object, AccessTools.Property(type, "SubGroupDelimiter").GetMethod);
            _getFormatDelegate = AccessTools3.GetDelegate<GetFormatDelegate>(@object, AccessTools.Property(type, nameof(MCMv3BaseSettings.Format)).GetMethod);
            _methodOnPropertyChangedDelegate = AccessTools3.GetDelegate<OnPropertyChangedDelegate>(@object, AccessTools.Method(type, nameof(MCMv3BaseSettings.OnPropertyChanged)));
            _methodGetAvailablePresetsDelegate = AccessTools3.GetDelegate<GetAvailablePresetsDelegate>(@object, AccessTools.Method(type, nameof(MCMv3BaseSettings.GetAvailablePresets)));
        }

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            _methodOnPropertyChangedDelegate?.Invoke(propertyName);

        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets() =>
            (_methodGetAvailablePresetsDelegate?.Invoke() ?? new Dictionary<string, Func<MCMv3BaseSettings>>())
            .Select(kv => new KeyValuePair<string, Func<BaseSettings>>(kv.Key, () => kv.Value() is { } val ? new MCMv3GlobalSettingsWrapper(val) : CreateNew()))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        protected override BaseSettings CreateNew() => new MCMv3GlobalSettingsWrapper(Activator.CreateInstance(Object.GetType())!);
    }
}