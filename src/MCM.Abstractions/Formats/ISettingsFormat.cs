using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;

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

        bool Save(BaseSettings settings, GameDirectory directory, string filename);
        BaseSettings Load(BaseSettings settings, GameDirectory directory, string filename);
    }
}