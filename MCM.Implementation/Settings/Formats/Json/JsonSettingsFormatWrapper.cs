using MCM.Abstractions.Settings.Formats;

namespace MCM.Implementation.Settings.Formats.Json
{
    public class JsonSettingsFormatWrapper : BaseSettingFormatWrapper, IJsonSettingsFormat
    {
        public JsonSettingsFormatWrapper(object @object) : base(@object) { }
    }
}