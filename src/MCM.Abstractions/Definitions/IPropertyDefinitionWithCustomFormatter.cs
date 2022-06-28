using System;

namespace MCM.Abstractions
{
    public interface IPropertyDefinitionWithCustomFormatter
    {
        Type? CustomFormatter { get; }
    }
}