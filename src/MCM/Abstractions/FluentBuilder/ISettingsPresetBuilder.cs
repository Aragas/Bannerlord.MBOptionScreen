using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder
{
    public interface ISettingsPresetBuilder
    {
        IDictionary<string, object?> PropertyValues { get; }

        ISettingsPresetBuilder SetPropertyValue(string property, object? value);
    }
}