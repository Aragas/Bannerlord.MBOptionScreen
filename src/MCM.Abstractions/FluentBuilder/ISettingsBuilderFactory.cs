namespace MCM.Abstractions.FluentBuilder
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsBuilderFactory
    {
        ISettingsBuilder Create(string id, string displayName);
    }
}