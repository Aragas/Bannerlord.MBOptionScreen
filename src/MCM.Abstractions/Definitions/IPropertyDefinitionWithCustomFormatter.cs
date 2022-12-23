using System;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionWithCustomFormatter
    {
        Type? CustomFormatter { get; }
    }
}