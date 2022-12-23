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
    sealed class PropertyDefinitionButtonWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionButton
    {
        public string Content { get; }

        public PropertyDefinitionButtonWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            Content = AccessTools2.Property(type, nameof(Content))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}