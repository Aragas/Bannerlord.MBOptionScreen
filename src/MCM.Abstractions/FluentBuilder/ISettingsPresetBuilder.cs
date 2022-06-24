using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Presets;

namespace MCM.Abstractions.FluentBuilder
{
    public interface ISettingsPresetBuilder
    {
        ISettingsPresetBuilder SetPropertyValue(string propertyName, object? value);

        ISettingsPreset Build(BaseSettings settings);
    }
}