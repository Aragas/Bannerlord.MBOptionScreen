namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionWithMinMax
    {
        decimal MinValue { get; }
        decimal MaxValue { get; }
    }
}