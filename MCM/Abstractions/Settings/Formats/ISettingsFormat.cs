using MCM.Abstractions.Settings.Base;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Formats
{
    public interface ISettingsFormat : IDependencyBase
    {
        IEnumerable<string> Extensions { get; }

        bool Save(BaseSettings settings, string path);
        BaseSettings? Load(BaseSettings settings, string path);
    }
}