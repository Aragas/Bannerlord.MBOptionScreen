using MCM.Abstractions.Attributes;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    [Version("e1.0.0", 1)]
    public class MemorySettingsFormat : IMemorySettingsFormat
    {
        public IEnumerable<string> Extensions { get; } = new string[] { "memory" };

        public BaseSettings? Load(BaseSettings settings, string path) => settings;
        public bool Save(BaseSettings settings, string path) => true;
    }
}