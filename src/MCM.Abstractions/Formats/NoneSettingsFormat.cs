using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

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
        public BaseSettings Load(BaseSettings settings, GameDirectory directory, string filename) => settings;

        /// <inheritdoc/>
        public bool Save(BaseSettings settings, GameDirectory directory, string filename) => true;
    }
}