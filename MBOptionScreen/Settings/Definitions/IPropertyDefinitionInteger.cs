namespace MBOptionScreen.Settings
{
    public interface IPropertyDefinitionInteger : IPropertyDefinitionBase
    {
        int MinValue { get; }
        int MaxValue { get; }
        string ValueFormat { get; }
    }
}