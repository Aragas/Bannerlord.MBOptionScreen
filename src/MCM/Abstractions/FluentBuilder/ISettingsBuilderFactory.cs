namespace MCM.Abstractions.FluentBuilder
{
    public interface ISettingsBuilderFactory
    {
        ISettingsBuilder Create(string id, string displayName);
    }
}