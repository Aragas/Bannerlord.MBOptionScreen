using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    public class MemorySettingsFormat : ISettingsFormat
    {
        public IEnumerable<string> Extensions { get; } = new string[] { "memory" };

        public SettingsBase? Load(SettingsBase settings, string path) => settings;
        public bool Save(SettingsBase settings, string path) => true;
    }
}