namespace MCM.Abstractions.FluentBuilder.Models
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyIntegerBuilder : ISettingsPropertyBuilder<ISettingsPropertyIntegerBuilder>
    {
        ISettingsPropertyBuilder AddValueFormat(string value);
    }
}