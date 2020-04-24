using System.Collections.Generic;

namespace MBOptionScreen.Settings
{
    public interface ISettingsFormat
    {
        IEnumerable<string> Extensions { get; }

        bool Save(SettingsBase settings, string path);
        SettingsBase? Load(SettingsBase settings, string path);
    }
}