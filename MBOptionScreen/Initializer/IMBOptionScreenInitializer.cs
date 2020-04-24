using TaleWorlds.Library;

namespace MBOptionScreen
{
    public interface IMBOptionScreenInitializer
    {
        void StartInitialization(ApplicationVersion gameVerion, bool first);

        void EndInitialization(bool first);
    }
}