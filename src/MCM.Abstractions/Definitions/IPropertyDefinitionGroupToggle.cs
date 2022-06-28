namespace MCM.Abstractions
{
    public interface IPropertyDefinitionGroupToggle : IPropertyDefinitionBase
    {
        bool IsToggle { get; }
    }
}