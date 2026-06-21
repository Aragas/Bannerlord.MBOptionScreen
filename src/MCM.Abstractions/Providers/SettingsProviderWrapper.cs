using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Base;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
    abstract class SettingsProviderWrapper : BaseSettingsProvider, IWrapper
    {
        private delegate IEnumerable GetSettingsDefinitionsDelegate();
        private delegate object? GetSettingsDelegate(string id);
        private delegate void SaveSettingsDelegate(object settings);
        private delegate void OverrideSettingsDelegate(object settings);
        private delegate void ResetSettingsDelegate(object settings);

        private readonly GetSettingsDefinitionsDelegate? _methodGetSettingsDefinitions;
        private readonly GetSettingsDelegate? _methodGetSettingsDelegate;
        private readonly SaveSettingsDelegate? _methodSaveSettingsDelegate;
        private readonly OverrideSettingsDelegate? _methodOverrideSettingsDelegate;
        private readonly ResetSettingsDelegate? _methodResetSettingsDelegate;

        public override IEnumerable<SettingsDefinition> SettingsDefinitions =>
            _methodGetSettingsDefinitions?.Invoke().Cast<object>().Select(x => new SettingsDefinitionWrapper(x)) ?? [];

        /// <inheritdoc />
        public object Object { get; }

        protected SettingsProviderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            _methodGetSettingsDefinitions = AccessTools2.GetPropertyGetterDelegate<GetSettingsDefinitionsDelegate>(@object, type, nameof(SettingsDefinitions));
            _methodGetSettingsDelegate = AccessTools2.GetDelegate<GetSettingsDelegate>(@object, type, nameof(GetSettings));
            _methodSaveSettingsDelegate = AccessTools2.GetDelegate<SaveSettingsDelegate>(@object, type, nameof(SaveSettings));
            _methodOverrideSettingsDelegate = AccessTools2.GetDelegate<OverrideSettingsDelegate>(@object, type, nameof(OverrideSettings));
            _methodResetSettingsDelegate = AccessTools2.GetDelegate<ResetSettingsDelegate>(@object, type, nameof(ResetSettings));
        }

        protected abstract BaseSettings? Create(object obj);
        protected abstract bool IsSettings(BaseSettings settings, [NotNullWhen(true)] out object? wrapped);

        /// <inheritdoc />
        public override BaseSettings? GetSettings(string id) => _methodGetSettingsDelegate?.Invoke(id) is { } obj ? Create(obj) : null;

        /// <inheritdoc />
        public override void SaveSettings(BaseSettings settings)
        {
            if (IsSettings(settings, out var wrapped))
                _methodSaveSettingsDelegate?.Invoke(wrapped);
        }

        /// <inheritdoc />
        public override void OverrideSettings(BaseSettings settings)
        {
            if (IsSettings(settings, out var wrapped))
                _methodOverrideSettingsDelegate?.Invoke(wrapped);
        }

        /// <inheritdoc />
        public override void ResetSettings(BaseSettings settings)
        {
            if (IsSettings(settings, out var wrapped))
                _methodResetSettingsDelegate?.Invoke(wrapped);
        }

        /// <inheritdoc />
        public abstract override IEnumerable<ISettingsPreset> GetPresets(string id);
    }
}