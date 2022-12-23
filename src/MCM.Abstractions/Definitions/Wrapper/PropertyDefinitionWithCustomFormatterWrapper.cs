using HarmonyLib.BUTR.Extensions;

using System;

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
    sealed class PropertyDefinitionWithCustomFormatterWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }

        public PropertyDefinitionWithCustomFormatterWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            CustomFormatter = AccessTools2.Property(type, nameof(CustomFormatter), false)?.GetValue(@object) as Type;
        }
    }
}