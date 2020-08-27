using System;

namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionWithActionFormat
    {
        Func<object, string>? ValueFormatFunc { get; }
    }
}