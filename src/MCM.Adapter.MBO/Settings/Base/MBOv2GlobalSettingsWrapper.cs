extern alias v2;
extern alias v4;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.ComponentModel;

using v4::MCM.Abstractions.Settings.Base;
using v4::MCM.Abstractions.Settings.Base.Global;

using MCMv2BaseSettings = v2::MBOptionScreen.Settings.SettingsBase;

namespace MCM.Adapter.MBO.Settings.Base
{
    internal sealed class MBOv2GlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        private delegate string GetIdDelegate();
        private delegate string GetFolderNameDelegate();
        private delegate string GetDisplayNameDelegate();
        private delegate int GetUIVersionDelegate();
        private delegate string GetSubFolderDelegate();
        private delegate char GetSubGroupDelimiterDelegate();
        private delegate string GetFormatDelegate();

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly GetFolderNameDelegate? _getFolderNameDelegate;
        private readonly GetDisplayNameDelegate? _getDisplayNameDelegate;
        private readonly GetUIVersionDelegate? _getUIVersionDelegate;
        private readonly GetSubFolderDelegate? _getSubFolderDelegate;
        private readonly GetSubGroupDelimiterDelegate? _getSubGroupDelimiterDelegate;
        private readonly GetFormatDelegate? _getFormatDelegate;

        public override string Id => _getIdDelegate?.Invoke() ?? "ERROR";
        public override string FolderName => _getFolderNameDelegate?.Invoke() ?? string.Empty;
        public override string DisplayName => _getDisplayNameDelegate?.Invoke() ?? "ERROR";
        public override int UIVersion => _getUIVersionDelegate?.Invoke() ?? 1;
        public override string SubFolder => _getSubFolderDelegate?.Invoke() ?? string.Empty;
        public override char SubGroupDelimiter => _getSubGroupDelimiterDelegate?.Invoke() ?? '/';
        public override string FormatType => _getFormatDelegate?.Invoke() ?? "json";
        public override string DiscoveryType => "mbo_v2_attributes";
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        public MBOv2GlobalSettingsWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            _getIdDelegate = AccessTools2.GetDelegate<GetIdDelegate>(@object, AccessTools.Property(type, nameof(MCMv2BaseSettings.Id)).GetMethod);
            _getFolderNameDelegate = AccessTools2.GetDelegate<GetFolderNameDelegate>(@object, AccessTools.Property(type, nameof(MCMv2BaseSettings.ModuleFolderName)).GetMethod);
            _getDisplayNameDelegate = AccessTools2.GetDelegate<GetDisplayNameDelegate>(@object, AccessTools.Property(type, nameof(MCMv2BaseSettings.ModName)).GetMethod);
            _getUIVersionDelegate = AccessTools2.GetDelegate<GetUIVersionDelegate>(@object, AccessTools.Property(type, nameof(MCMv2BaseSettings.UIVersion)).GetMethod);
            _getSubFolderDelegate = AccessTools2.GetDelegate<GetSubFolderDelegate>(@object, AccessTools.Property(type, nameof(MCMv2BaseSettings.SubFolder)).GetMethod);
            _getSubGroupDelimiterDelegate = AccessTools2.GetDelegate<GetSubGroupDelimiterDelegate>(@object, AccessTools.Property(type, "SubGroupDelimiter").GetMethod);
            _getFormatDelegate = AccessTools2.GetDelegate<GetFormatDelegate>(@object, AccessTools.Property(type, nameof(MCMv2BaseSettings.Format)).GetMethod);
        }

        protected override BaseSettings CreateNew() => new MBOv2GlobalSettingsWrapper(Activator.CreateInstance(Object.GetType())!);

        internal void UpdateReference(object @object) => Object = @object;
    }
}