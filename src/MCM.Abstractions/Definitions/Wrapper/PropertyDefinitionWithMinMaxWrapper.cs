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
    sealed class PropertyDefinitionWithMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithMinMax
    {
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }

        public PropertyDefinitionWithMinMaxWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            MinValue = AccessTools2.Property(type, nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = AccessTools2.Property(type, nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}