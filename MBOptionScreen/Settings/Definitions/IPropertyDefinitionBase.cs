namespace MBOptionScreen.Settings
{
    public interface IPropertyDefinitionBase
    {
        string DisplayName { get; }
        int Order { get; }
        bool RequireRestart { get; }
        string HintText { get; }
    }
}