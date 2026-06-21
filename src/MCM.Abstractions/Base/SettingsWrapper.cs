using HarmonyLib.BUTR.Extensions;

using MCM.Common;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MCM.Abstractions.Base.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class SettingsWrapper : BaseSettings, IWrapper
    {
        private delegate string GetIdDelegate();
        private delegate string GetFolderNameDelegate();
        private delegate string GetDisplayNameDelegate();
        private delegate int GetUIVersionDelegate();
        private delegate string GetSubFolderDelegate();
        private delegate char GetSubGroupDelimiterDelegate();
        private delegate string GetFormatDelegate();
        private delegate void OnPropertyChangedDelegate(string? propertyName);
        private delegate IEnumerable? GetBuiltInPresetsDelegate();
        private delegate object CreateNewDelegate();
        private delegate object CopyAsNewDelegate();

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly GetFolderNameDelegate? _getFolderNameDelegate;
        private readonly GetDisplayNameDelegate? _getDisplayNameDelegate;
        private readonly GetUIVersionDelegate? _getUIVersionDelegate;
        private readonly GetSubFolderDelegate? _getSubFolderDelegate;
        private readonly GetSubGroupDelimiterDelegate? _getSubGroupDelimiterDelegate;
        private readonly GetFormatDelegate? _getFormatDelegate;
        private readonly OnPropertyChangedDelegate? _methodOnPropertyChangedDelegate;
        private readonly GetBuiltInPresetsDelegate? _methodGetBuiltInPresetsDelegate;
        private readonly CreateNewDelegate? _methodCreateNewDelegate;
        private readonly CopyAsNewDelegate? _methodCopyAsNewDelegate;

        /// <inheritdoc />
        public override string Id => _getIdDelegate?.Invoke() ?? "ERROR";
        /// <inheritdoc />
        public override string FolderName => _getFolderNameDelegate?.Invoke() ?? string.Empty;
        /// <inheritdoc />
        public override string DisplayName => _getDisplayNameDelegate?.Invoke() ?? "ERROR";
        /// <inheritdoc />
        public override int UIVersion => _getUIVersionDelegate?.Invoke() ?? 1;
        /// <inheritdoc />
        public override string SubFolder => _getSubFolderDelegate?.Invoke() ?? string.Empty;
        /// <inheritdoc />
        public override char SubGroupDelimiter => _getSubGroupDelimiterDelegate?.Invoke() ?? '/';
        /// <inheritdoc />
        public override string FormatType => _getFormatDelegate?.Invoke() ?? "none";
        /// <inheritdoc />
        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        /// <inheritdoc/>
        public object? Object { get; }

        protected SettingsWrapper(object? @object)
        {
            if (@object is null) return;

            Object = @object;
            var type = @object.GetType();

            _getIdDelegate = AccessTools2.GetPropertyGetterDelegate<GetIdDelegate>(@object, type, nameof(Id));
            _getFolderNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetFolderNameDelegate>(@object, type, nameof(FolderName));
            _getDisplayNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetDisplayNameDelegate>(@object, type, nameof(DisplayName));
            _getUIVersionDelegate = AccessTools2.GetPropertyGetterDelegate<GetUIVersionDelegate>(@object, type, nameof(UIVersion));
            _getSubFolderDelegate = AccessTools2.GetPropertyGetterDelegate<GetSubFolderDelegate>(@object, type, nameof(SubFolder));
            _getSubGroupDelimiterDelegate = AccessTools2.GetPropertyGetterDelegate<GetSubGroupDelimiterDelegate>(@object, type, nameof(SubGroupDelimiter));
            _getFormatDelegate = AccessTools2.GetPropertyGetterDelegate<GetFormatDelegate>(@object, type, nameof(FormatType));
            _methodOnPropertyChangedDelegate = AccessTools2.GetDelegate<OnPropertyChangedDelegate>(@object, type, nameof(OnPropertyChanged));
            _methodGetBuiltInPresetsDelegate = AccessTools2.GetDelegate<GetBuiltInPresetsDelegate>(@object, type, nameof(GetBuiltInPresets));
            _methodCreateNewDelegate = AccessTools2.GetDelegate<CreateNewDelegate>(@object, type, nameof(CreateNew));
            _methodCopyAsNewDelegate = AccessTools2.GetDelegate<CopyAsNewDelegate>(@object, type, nameof(CopyAsNew));
        }

        protected abstract BaseSettings Create(object? @object);
        protected abstract ISettingsPreset CreatePreset(object? @object);

        /// <inheritdoc/>
        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            _methodOnPropertyChangedDelegate?.Invoke(propertyName);

        public override IEnumerable<ISettingsPreset> GetBuiltInPresets() =>
            _methodGetBuiltInPresetsDelegate?.Invoke()?.Cast<object>().Select(CreatePreset).OfType<ISettingsPreset>() ?? [];

        public override BaseSettings CreateNew() => Create(_methodCreateNewDelegate?.Invoke());
        public override BaseSettings CopyAsNew() => Create(_methodCopyAsNewDelegate?.Invoke());
    }
}