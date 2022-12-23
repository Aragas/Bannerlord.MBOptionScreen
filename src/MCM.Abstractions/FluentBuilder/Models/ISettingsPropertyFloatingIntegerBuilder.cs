namespace MCM.Abstractions.FluentBuilder.Models
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyFloatingIntegerBuilder : ISettingsPropertyBuilder<ISettingsPropertyFloatingIntegerBuilder>
    {
        ISettingsPropertyBuilder AddValueFormat(string value);
    }
}