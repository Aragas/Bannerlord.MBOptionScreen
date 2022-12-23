using HarmonyLib.BUTR.Extensions;

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
    sealed class PropertyDefinitionWithFormatWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithFormat
    {
        /// <inheritdoc/>
        public string ValueFormat { get; }

        public PropertyDefinitionWithFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormat = AccessTools2.Property(type, nameof(ValueFormat))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}