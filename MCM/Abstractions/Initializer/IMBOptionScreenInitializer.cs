using TaleWorlds.Library;

namespace MCM.Abstractions.Initializer
{
    public interface IMBOptionScreenInitializer
    {
        void StartInitialization(ApplicationVersion gameVersion, bool first);

        void EndInitialization(bool first);
    }
}