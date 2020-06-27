namespace MCM.Abstractions.FluentBuilder.Models
{
    public interface ISettingsPropertyIntegerBuilder : ISettingsPropertyBuilder<ISettingsPropertyIntegerBuilder>
    {
        ISettingsPropertyBuilder AddValueFormat(string value);
    }
}