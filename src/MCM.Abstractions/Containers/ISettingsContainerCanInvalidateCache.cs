using System;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsContainerCanInvalidateCache
    {
        event Action InstanceCacheInvalidated;
    }
}