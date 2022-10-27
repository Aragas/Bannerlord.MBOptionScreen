using System;

namespace MCM.Abstractions
{
    public interface IPropertyDefinitionWithActionFormat
    {
        Func<object, string>? ValueFormatFunc { get; }
    }
}