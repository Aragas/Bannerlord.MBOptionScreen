namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionGroupToggle : IPropertyDefinitionBase
    {
        bool IsToggle { get; }
    }
}