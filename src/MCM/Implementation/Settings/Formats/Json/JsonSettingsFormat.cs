using BUTR.DependencyInjection.Logger;

namespace MCM.Implementation.Settings.Formats.Json
{
    internal sealed class JsonSettingsFormat : BaseJsonSettingsFormat, IJsonSettingsFormat
    {
        public JsonSettingsFormat(IBUTRLogger<JsonSettingsFormat> logger) : base(logger) { }
    }
}