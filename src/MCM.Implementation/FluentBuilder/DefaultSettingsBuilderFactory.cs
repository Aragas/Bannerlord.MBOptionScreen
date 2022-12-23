using MCM.Abstractions.FluentBuilder;

namespace MCM.Implementation.FluentBuilder
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class DefaultSettingsBuilderFactory : ISettingsBuilderFactory
    {
        public ISettingsBuilder Create(string id, string displayName) => new DefaultSettingsBuilder(id, displayName);
    }
}