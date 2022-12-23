using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;

namespace MCM.Common
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    readonly ref struct SelectedIndexWrapper
    {
        private static readonly ConcurrentDictionary<Type, GetSelectedIndexDelegate?> _getSelectedIndexCache = new();
        private static readonly ConcurrentDictionary<Type, SetSelectedIndexDelegate?> _setSelectedIndexCache = new();

        private delegate int GetSelectedIndexDelegate(object instance);
        private delegate void SetSelectedIndexDelegate(object instance, int value);

        private readonly GetSelectedIndexDelegate? _getSelectedIndex;
        private readonly SetSelectedIndexDelegate? _setSelectedIndex;

        private readonly object? _object;

        public int SelectedIndex
        {
            get => _getSelectedIndex?.Invoke(_object!) ?? -1;
            set => _setSelectedIndex?.Invoke(_object!, value);
        }

        public SelectedIndexWrapper(object? @object)
        {
            _object = @object;
            var type = @object?.GetType();

            _getSelectedIndex = type is not null
                ? _getSelectedIndexCache.GetOrAdd(type, static t => AccessTools2.GetPropertyGetterDelegate<GetSelectedIndexDelegate>(t, "SelectedIndex"))
                : null;
            _setSelectedIndex = type is not null
                ? _setSelectedIndexCache.GetOrAdd(type, static t => AccessTools2.GetPropertySetterDelegate<SetSelectedIndexDelegate>(t, "SelectedIndex"))
                : null;
        }
    }
}