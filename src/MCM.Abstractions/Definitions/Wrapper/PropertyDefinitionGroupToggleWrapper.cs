using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Wrapper
{
    public sealed class PropertyDefinitionGroupToggleWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupToggle
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