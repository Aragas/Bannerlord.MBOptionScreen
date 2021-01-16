namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyGroupDefinition
    {
        string GroupName { get; }
        bool IsMainToggle { get; }
        int GroupOrder { get; }
    }
}