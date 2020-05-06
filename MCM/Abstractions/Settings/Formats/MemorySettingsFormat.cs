using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    public class MemorySettingsFormat : ISettingsFormat
    {
        public IEnumerable<string> Extensions { get; } = new string[] { "memory" };

        public BaseSettings? Load(BaseSettings settings, string path) => settings;
        public bool Save(BaseSettings settings, string path) => true;
    }
}