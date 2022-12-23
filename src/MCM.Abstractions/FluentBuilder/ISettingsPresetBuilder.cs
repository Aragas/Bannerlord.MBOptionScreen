using MCM.Abstractions.Base;

namespace MCM.Abstractions.FluentBuilder
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPresetBuilder
    {
        ISettingsPresetBuilder SetPropertyValue(string propertyName, object? value);

        ISettingsPreset Build(BaseSettings settings);
    }
}