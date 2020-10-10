using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;

namespace MCM.Implementation.Settings.Formats.Json2
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IJson2SettingsFormat : ISettingsFormat
    {
        string SaveJson(BaseSettings settings);
        BaseSettings? LoadFromJson(BaseSettings settings, string content);
    }
}