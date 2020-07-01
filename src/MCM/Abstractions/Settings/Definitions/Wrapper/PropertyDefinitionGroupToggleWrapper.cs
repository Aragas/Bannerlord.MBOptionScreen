using HarmonyLib;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionGroupToggleWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionGroupToggle
    {
        /// <inheritdoc/>
        public bool IsToggle { get; }

        public PropertyDefinitionGroupToggleWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            IsToggle = AccessTools.Property(type, nameof(IsToggle))?.GetValue(@object) as bool? ?? false;
        }
    }
}