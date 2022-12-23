namespace MCM.Abstractions.Properties
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IAttributeSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer { }
}