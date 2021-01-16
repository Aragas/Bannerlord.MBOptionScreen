using TaleWorlds.Library;

namespace MCM.Abstractions
{
    public interface IVersion
    {
        ApplicationVersion GameVersion { get; }
        int ImplementationVersion { get; }
    }
}