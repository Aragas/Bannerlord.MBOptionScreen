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
    sealed class PropertyDefinitionWithActionFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithActionFormat
    {
        /// <inheritdoc/>
        public Func<object, string> ValueFormatFunc { get; }

        public PropertyDefinitionWithActionFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormatFunc = AccessTools2.Property(type, nameof(ValueFormatFunc))?.GetValue(@object) as Func<object, string> ?? (obj => obj.ToString());
        }
    }
}