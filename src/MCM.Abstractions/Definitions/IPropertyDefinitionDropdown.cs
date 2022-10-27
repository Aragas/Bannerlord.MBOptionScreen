namespace MCM.Abstractions
{
    public interface IPropertyDefinitionDropdown : IPropertyDefinitionBase
    {
        int SelectedIndex { get; }
    }
}