using System;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        enum ExternalSettingsProviderInvalidateCacheType
    {
        None,
        Global,
        PerCampaign,
        PerSave,
    }
    
    /// <summary>
    /// Used to add foreign Options API's that MCM will be able to use.
    /// Most likely will be used to ease backwards compatibility ports of older MCM API's so we'll be able to reuse more code.
    /// This is a higher level alternative to using <see cref="ISettingsContainer"/>.
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IExternalSettingsProviderCanInvalidateCache
    {
        event Action<ExternalSettingsProviderInvalidateCacheType> InstanceCacheInvalidated;
    }
}