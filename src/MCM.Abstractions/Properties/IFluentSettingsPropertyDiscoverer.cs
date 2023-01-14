namespace MCM.Abstractions.Properties
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IFluentSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer
    { }
}