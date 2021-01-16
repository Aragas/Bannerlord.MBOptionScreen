using TaleWorlds.Library;

namespace MCM.Utils
{
    public static class ApplicationVersionUtils
    {
        public static bool TryParse(string versionAsString, out ApplicationVersion version)
        {
            version = default;
            return false;
        }

        public static bool IsSameOverride(this ApplicationVersion @this, ApplicationVersion other) => false;

        public static ApplicationVersion GameVersion() => default;
    }
}