using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Base;
using MCM.Common;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class SettingsPresetWrapper<TSetting> : ISettingsPreset, IWrapper where TSetting : BaseSettings, IWrapper
    {
        private delegate string GetSettingsIdDelegate();
        private delegate string GetIdDelegate();
        private delegate string GetNameDelegate();
        private delegate object LoadPresetDelegate();
        private delegate bool SavePresetDelegate(object settings);

        private readonly GetSettingsIdDelegate? _methodGetSettingsIdDelegate;
        private readonly GetIdDelegate? _methodGetIdDelegate;
        private readonly GetNameDelegate? _methodGetNameDelegate;
        private readonly LoadPresetDelegate? _methodLoadPresetDelegate;
        private readonly SavePresetDelegate? _methodSavePresetDelegate;

        /// <inheritdoc />
        public string SettingsId => _methodGetSettingsIdDelegate?.Invoke() ?? "ERROR";

        /// <inheritdoc />
        public string Id => _methodGetIdDelegate?.Invoke() ?? "ERROR";

        /// <inheritdoc />
        public string Name => _methodGetNameDelegate?.Invoke() ?? "ERROR";

        /// <inheritdoc />
        public object? Object { get; }

        protected SettingsPresetWrapper(object? @object)
        {
            if (@object is null) return;
            
            Object = @object;
            var type = @object.GetType();

            _methodGetSettingsIdDelegate = AccessTools2.GetPropertyGetterDelegate<GetSettingsIdDelegate>(@object, type, nameof(SettingsId));
            _methodGetIdDelegate = AccessTools2.GetPropertyGetterDelegate<GetIdDelegate>(@object, type, nameof(Id));
            _methodGetNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetNameDelegate>(@object, type, nameof(Name));
            _methodLoadPresetDelegate = AccessTools2.GetDelegate<LoadPresetDelegate>(@object, type, nameof(LoadPreset));
            _methodSavePresetDelegate = AccessTools2.GetDelegate<SavePresetDelegate>(@object, type, nameof(SavePreset));
        }

        protected abstract TSetting Create(object? @object);

        /// <inheritdoc />
        public BaseSettings LoadPreset() => Create(_methodLoadPresetDelegate?.Invoke());

        /// <inheritdoc />
        public bool SavePreset(BaseSettings settings) => settings is TSetting { Object: { } obj } && (_methodSavePresetDelegate?.Invoke(obj) ?? false);
    }
}