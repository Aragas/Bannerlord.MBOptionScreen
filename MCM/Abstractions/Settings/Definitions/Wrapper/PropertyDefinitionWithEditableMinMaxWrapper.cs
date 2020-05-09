namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithEditableMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithEditableMinMax
    {
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }

        public PropertyDefinitionWithEditableMinMaxWrapper(object @object) : base(@object)
        {
            EditableMinValue = @object.GetType().GetProperty(nameof(EditableMinValue))?.GetValue(@object) as decimal? ?? 0;
            EditableMaxValue = @object.GetType().GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}