namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionWithEditableMinMax
    {
        decimal EditableMinValue { get; }
        decimal EditableMaxValue { get; }
    }
}