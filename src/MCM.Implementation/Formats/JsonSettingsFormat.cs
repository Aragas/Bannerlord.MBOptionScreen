using BUTR.DependencyInjection.Logger;

using System.Collections.Generic;

namespace MCM.Implementation.Settings.Formats
{
    public sealed class JsonSettingsFormat : BaseJsonSettingsFormat
    {
        public override IEnumerable<string> FormatTypes => new[] { "json", "json2" };

        public JsonSettingsFormat(IBUTRLogger<JsonSettingsFormat> logger) : base(logger) { }
    }
}