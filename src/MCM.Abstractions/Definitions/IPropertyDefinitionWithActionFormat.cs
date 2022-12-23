using System;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionWithActionFormat
    {
        Func<object, string>? ValueFormatFunc { get; }
    }
}