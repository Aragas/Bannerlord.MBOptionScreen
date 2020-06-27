using System;

namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionWithCustomFormatter
    {
        Type? CustomFormatter { get; }
    }
}