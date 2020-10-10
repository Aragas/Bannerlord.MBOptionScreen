using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;

namespace MCM.Implementation.Settings.Formats.Json
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IJsonSettingsFormat : ISettingsFormat
    {
        string SaveJson(BaseSettings settings);
        BaseSettings? LoadFromJson(BaseSettings settings, string content);
    }
}