using MCM.Abstractions.Settings.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    public sealed class MemorySettingsFormat : ISettingsFormat
    {
        /// <inheritdoc/>
        public IEnumerable<string> FormatTypes { get; } = new [] { "memory" };

        /// <inheritdoc/>
        public BaseSettings? Load(BaseSettings settings, string directoryPath, string filename) => settings;
        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string directoryPath, string filename) => true;
    }
}