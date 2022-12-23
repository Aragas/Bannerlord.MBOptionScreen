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
    sealed class PropertyDefinitionGroupToggleWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupToggle
    {
        /// <inheritdoc/>
        public bool IsToggle { get; }

        public PropertyDefinitionGroupToggleWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            IsToggle = AccessTools2.Property(type, nameof(IsToggle))?.GetValue(@object) as bool? ?? false;
        }
    }
}