namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithEditableMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithEditableMinMax
    {
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }

        public PropertyDefinitionWithEditableMinMaxWrapper(object @object) : base(@object) { }
    }
}