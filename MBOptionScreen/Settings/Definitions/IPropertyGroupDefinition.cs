namespace MBOptionScreen.Settings
{
    public interface IPropertyGroupDefinition
    {
        string GroupName { get; }
        bool IsMainToggle { get; }
        int Order { get; }
    }
}