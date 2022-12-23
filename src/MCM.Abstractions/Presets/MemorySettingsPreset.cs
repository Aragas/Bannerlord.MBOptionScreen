using MCM.Abstractions.Base;

using System;

namespace MCM.Abstractions
{
    /// <summary>
    /// A readonly non serializable preset
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class MemorySettingsPreset : ISettingsPreset
    {
        private readonly Func<BaseSettings> _template;

        /// <inheritdoc />
        public string SettingsId { get; }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        public MemorySettingsPreset(string settingId, string id, string name, Func<BaseSettings> template)
        {
            SettingsId = settingId;
            Id = id;
            Name = name;
            _template = template;
        }

        /// <inheritdoc />
        public BaseSettings LoadPreset() => _template();

        /// <inheritdoc />
        public bool SavePreset(BaseSettings settings) => true;
    }
}