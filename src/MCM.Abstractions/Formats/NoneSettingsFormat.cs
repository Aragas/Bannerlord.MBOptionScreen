using MCM.Abstractions.Base;

using System.Collections.Generic;

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
    sealed class NoneSettingsFormat : ISettingsFormat
    {
        /// <inheritdoc/>
        public IEnumerable<string> FormatTypes { get; } = new[] { "none" };

        /// <inheritdoc/>
        public BaseSettings Load(BaseSettings settings, string directoryPath, string filename) => settings;

        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string directoryPath, string filename) => true;
    }
}