using MCM.Abstractions.Base;

using System.Collections.Generic;
using System.IO;

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
    sealed class MemorySettingsFormat : ISettingsFormat
    {
        private readonly Dictionary<string, BaseSettings> _settings = new();

        /// <inheritdoc/>
        public IEnumerable<string> FormatTypes { get; } = new[] { "memory" };

        /// <inheritdoc/>
        public BaseSettings Load(BaseSettings settings, string directoryPath, string filename)
        {
            if (_settings.TryGetValue(Path.Combine(directoryPath, filename), out var sett) || !ReferenceEquals(settings, sett))
                SettingsUtils.OverrideSettings(settings, sett!);
            return settings;
        }

        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string directoryPath, string filename)
        {
            _settings[Path.Combine(directoryPath, filename)] = settings;
            return true;
        }
    }
}