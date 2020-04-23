namespace MBOptionScreen.Settings
{
    public interface IPropertyDefinitionFloatingInteger : IPropertyDefinitionBase
    {
        float MinValue { get; }
        float MaxValue { get; }
        string ValueFormat { get; }
    }
}