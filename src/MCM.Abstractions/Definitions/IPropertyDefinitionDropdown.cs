namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionDropdown : IPropertyDefinitionBase
    {
        int SelectedIndex { get; }
    }
}