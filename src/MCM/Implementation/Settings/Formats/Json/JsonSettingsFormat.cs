using Microsoft.Extensions.Logging;

namespace MCM.Implementation.Settings.Formats.Json
{
    internal sealed class JsonSettingsFormat : BaseJsonSettingsFormat, IJsonSettingsFormat
    {
        public JsonSettingsFormat(ILogger<JsonSettingsFormat> logger) : base(logger) { }
    }
}