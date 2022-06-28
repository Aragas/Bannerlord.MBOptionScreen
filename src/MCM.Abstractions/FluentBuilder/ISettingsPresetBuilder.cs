using MCM.Abstractions.Base;

namespace MCM.Abstractions.FluentBuilder
{
    public interface ISettingsPresetBuilder
    {
        ISettingsPresetBuilder SetPropertyValue(string propertyName, object? value);

        ISettingsPreset Build(BaseSettings settings);
    }
}