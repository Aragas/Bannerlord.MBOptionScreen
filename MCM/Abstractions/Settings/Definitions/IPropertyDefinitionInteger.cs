namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionInteger : IPropertyDefinitionBase
    {
        int MinValue { get; }
        int MaxValue { get; }
        string ValueFormat { get; }
    }
}