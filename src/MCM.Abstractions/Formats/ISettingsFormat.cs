using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
    public interface ISettingsFormat
    {
        IEnumerable<string> FormatTypes { get; }

        bool Save(BaseSettings settings, string directoryPath, string filename);
        BaseSettings Load(BaseSettings settings, string directoryPath, string filename);
    }
}