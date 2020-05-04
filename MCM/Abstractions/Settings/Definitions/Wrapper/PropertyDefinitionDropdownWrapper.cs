namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionDropdownWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionDropdown
    {
        public int SelectedIndex { get; }

        public PropertyDefinitionDropdownWrapper(object @object) : base(@object)
        {
            SelectedIndex = @object.GetType().GetProperty(nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
        }
    }
}