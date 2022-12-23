using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsFormat
    {
        IEnumerable<string> FormatTypes { get; }

        bool Save(BaseSettings settings, string directoryPath, string filename);
        BaseSettings Load(BaseSettings settings, string directoryPath, string filename);
    }
}