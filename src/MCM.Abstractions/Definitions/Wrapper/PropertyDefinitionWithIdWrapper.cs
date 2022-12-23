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
    sealed class PropertyDefinitionWithIdWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithId
    {
        /// <inheritdoc/>
        public string Id { get; }

        public PropertyDefinitionWithIdWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            Id = AccessTools2.Property(type, nameof(Id))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}