extern alias v3;
extern alias v4;

using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using v4::MCM.Abstractions.Settings.Base;
using v4::MCM.Abstractions.Settings.Base.Global;

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

            _getIdDelegate = AccessTools2.GetPropertyGetterDelegate<GetIdDelegate>(@object, type, nameof(MCMv3BaseSettings.Id));
            _getFolderNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetFolderNameDelegate>(@object, type, nameof(MCMv3BaseSettings.FolderName));
            _getDisplayNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetDisplayNameDelegate>(@object, type, nameof(MCMv3BaseSettings.DisplayName));
            _getUIVersionDelegate = AccessTools2.GetPropertyGetterDelegate<GetUIVersionDelegate>(@object, type, nameof(MCMv3BaseSettings.UIVersion));
            _getSubFolderDelegate = AccessTools2.GetPropertyGetterDelegate<GetSubFolderDelegate>(@object, type, nameof(MCMv3BaseSettings.SubFolder));
            _getSubGroupDelimiterDelegate = AccessTools2.GetPropertyGetterDelegate<GetSubGroupDelimiterDelegate>(@object, type, "SubGroupDelimiter");
            _getFormatDelegate = AccessTools2.GetPropertyGetterDelegate<GetFormatDelegate>(@object, type, nameof(MCMv3BaseSettings.Format));
            _methodOnPropertyChangedDelegate = AccessTools2.GetDelegate<OnPropertyChangedDelegate>(@object, type, nameof(MCMv3BaseSettings.OnPropertyChanged));
            _methodGetAvailablePresetsDelegate = AccessTools2.GetDelegate<GetAvailablePresetsDelegate>(@object, type, nameof(MCMv3BaseSettings.GetAvailablePresets));
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