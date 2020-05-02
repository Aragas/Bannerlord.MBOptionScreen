namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionFloatingInteger : IPropertyDefinitionBase
    {
        float MinValue { get; }
        float MaxValue { get; }
        string ValueFormat { get; }
    }
}