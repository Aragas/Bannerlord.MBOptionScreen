namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionWithEditableMinMax
    {
        decimal EditableMinValue { get; }
        decimal EditableMaxValue { get; }
    }
}