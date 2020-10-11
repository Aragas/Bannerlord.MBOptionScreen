using MCM.Abstractions.Settings.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    public sealed class NoneSettingsFormat : ISettingsFormat
    {
        private readonly Dictionary<string, BaseSettings> _settings = new Dictionary<string, BaseSettings>();

        /// <inheritdoc/>
        public IEnumerable<string> FormatTypes { get; } = new [] { "none" };

        /// <inheritdoc/>
        public BaseSettings Load(BaseSettings settings, string directoryPath, string filename) => settings;

        /// <inheritdoc/>
        public bool Save(BaseSettings settings, string directoryPath, string filename) => true;
    }
}