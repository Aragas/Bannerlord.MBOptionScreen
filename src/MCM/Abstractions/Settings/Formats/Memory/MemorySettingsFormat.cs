using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats.Memory
{
    [Version("e1.0.0", 1)]
    public class MemorySettingsFormat : IMemorySettingsFormat
    {
        /// <inheritdoc/>
        public IEnumerable<string> Extensions { get; } = new [] { "memory" };

        /// <inheritdoc/>
        public BaseSettings? Load(BaseSettings settings, string path) => settings;
        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string path) => true;
    }
}