using MCM.Logger;

namespace MCM.Implementation.Settings.Formats.Json
{
    internal sealed class JsonSettingsFormat : BaseJsonSettingsFormat, IJsonSettingsFormat
    {
        public JsonSettingsFormat(IMCMLogger<JsonSettingsFormat> logger) : base(logger) { }
    }
}