using MCM.Abstractions.Settings.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats.Memory
{
    public sealed class MemorySettingsFormat : IMemorySettingsFormat
    {
        /// <inheritdoc/>
        public IEnumerable<string> Extensions { get; } = new [] { "memory" };

        /// <inheritdoc/>
        public BaseSettings? Load(BaseSettings settings, string directoryPath, string filename) => settings;
        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string directoryPath, string filename) => true;
    }
}