using HarmonyLib;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionDropdownWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionDropdown
    {
        /// <inheritdoc/>
        public int SelectedIndex { get; }

        public PropertyDefinitionDropdownWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            SelectedIndex = AccessTools.Property(type, nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
        }
    }
}