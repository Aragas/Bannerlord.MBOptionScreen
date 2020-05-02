using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    public interface ISettingsFormat
    {
        IEnumerable<string> Extensions { get; }

        bool Save(SettingsBase settings, string path);
        SettingsBase? Load(SettingsBase settings, string path);
    }
}