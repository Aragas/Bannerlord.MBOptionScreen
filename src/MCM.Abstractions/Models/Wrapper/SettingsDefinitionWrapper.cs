namespace MCM.Abstractions.Wrapper
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class SettingsDefinitionWrapper : SettingsDefinition
    {
        private static string? GetSettingsId(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(SettingsId));
            return propInfo?.GetValue(@object) as string;
        }

        public SettingsDefinitionWrapper(object @object) : base(GetSettingsId(@object) ?? "ERROR") { }
    }
}