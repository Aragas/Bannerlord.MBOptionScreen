namespace MCM.Abstractions
{
    public interface IPropertyDefinitionWithEditableMinMax
    {
        decimal EditableMinValue { get; }
        decimal EditableMaxValue { get; }
    }
}