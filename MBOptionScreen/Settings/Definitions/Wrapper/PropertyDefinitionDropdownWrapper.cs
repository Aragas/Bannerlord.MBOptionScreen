namespace MBOptionScreen.Settings
{
    internal sealed class PropertyDefinitionDropdownWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionDropdown
    {
        public int SelectedIndex { get; }

        internal PropertyDefinitionDropdownWrapper(object @object) : base(@object)
        {
            SelectedIndex = @object.GetType().GetProperty("SelectedIndex")?.GetValue(@object) as int? ?? 0;
        }
    }
}