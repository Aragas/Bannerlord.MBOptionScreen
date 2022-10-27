using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
    public sealed class NoneSettingsFormat : ISettingsFormat
    {
        /// <inheritdoc/>
        public IEnumerable<string> FormatTypes { get; } = new[] { "none" };

        /// <inheritdoc/>
        public BaseSettings Load(BaseSettings settings, string directoryPath, string filename) => settings;

        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string directoryPath, string filename) => true;
    }
}