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
    sealed class PropertyDefinitionDropdownWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionDropdown
    {
        /// <inheritdoc/>
        public int SelectedIndex { get; }

        public PropertyDefinitionDropdownWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            SelectedIndex = AccessTools2.Property(type, nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
        }
    }
}