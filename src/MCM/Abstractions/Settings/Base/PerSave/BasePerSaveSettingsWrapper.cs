using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Settings.Base.PerSave
{
    public abstract class BasePerSaveSettingsWrapper : PerSaveSettings, IWrapper
    {
        private delegate string GetIdDelegate();
        private delegate string GetFolderNameDelegate();
        private delegate string GetDisplayNameDelegate();
        private delegate int GetUIVersionDelegate();
        private delegate string GetSubFolderDelegate();
        private delegate char GetSubGroupDelimiterDelegate();
        private delegate void OnPropertyChangedDelegate(string? propertyName);

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly GetFolderNameDelegate? _getFolderNameDelegate;
        private readonly GetDisplayNameDelegate? _getDisplayNameDelegate;
        private readonly GetUIVersionDelegate? _getUIVersionDelegate;
        private readonly GetSubFolderDelegate? _getSubFolderDelegate;
        private readonly GetSubGroupDelimiterDelegate? _getSubGroupDelimiterDelegate;
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
        public override char SubGroupDelimiter => _getSubGroupDelimiterDelegate?.Invoke() ?? '/';
        /// <inheritdoc/>
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        protected BasePerSaveSettingsWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            _getIdDelegate = AccessTools2.GetDelegate<GetIdDelegate>(@object, AccessTools.Property(type, nameof(Id)).GetGetMethod());
            _getFolderNameDelegate = AccessTools2.GetDelegate<GetFolderNameDelegate>(@object, AccessTools.Property(type, nameof(FolderName)).GetGetMethod());
            _getDisplayNameDelegate = AccessTools2.GetDelegate<GetDisplayNameDelegate>(@object, AccessTools.Property(type, nameof(DisplayName)).GetGetMethod());
            _getUIVersionDelegate = AccessTools2.GetDelegate<GetUIVersionDelegate>(@object, AccessTools.Property(type, nameof(UIVersion)).GetGetMethod());
            _getSubFolderDelegate = AccessTools2.GetDelegate<GetSubFolderDelegate>(@object, AccessTools.Property(type, nameof(SubFolder)).GetGetMethod());
            _getSubGroupDelimiterDelegate = AccessTools2.GetDelegate<GetSubGroupDelimiterDelegate>(@object, AccessTools.Property(type, nameof(SubGroupDelimiter)).GetGetMethod());
            _methodOnPropertyChangedDelegate = AccessTools2.GetDelegate<OnPropertyChangedDelegate>(@object, AccessTools.Method(type, nameof(OnPropertyChanged)));
        }

        /// <inheritdoc/>
        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            _methodOnPropertyChangedDelegate?.Invoke(propertyName);
    }
}