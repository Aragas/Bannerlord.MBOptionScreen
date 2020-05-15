namespace MCM.Abstractions.FluentBuilder.Models
{
    public interface ISettingsPropertyFloatingIntegerBuilder : ISettingsPropertyBuilder<ISettingsPropertyFloatingIntegerBuilder>
    {
        ISettingsPropertyBuilder AddValueFormat(string value);
    }
}