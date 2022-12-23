using BUTR.DependencyInjection.Logger;

using System.Collections.Generic;

namespace MCM.Implementation
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class JsonSettingsFormat : BaseJsonSettingsFormat
    {
        public override IEnumerable<string> FormatTypes => new[] { "json", "json2" };

        public JsonSettingsFormat(IBUTRLogger<JsonSettingsFormat> logger) : base(logger) { }
    }
}