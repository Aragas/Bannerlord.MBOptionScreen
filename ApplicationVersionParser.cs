using TaleWorlds.Library;

namespace MBOptionScreen
{
    internal static class ApplicationVersionParser
    {
        public static bool TryParse(string versionAsString, out ApplicationVersion version)
        {
            version = default;

            var array = versionAsString.Split('.');
            if (array.Length != 3 && array.Length != 4)
                return false;

            var applicationVersionType = ApplicationVersion.ApplicationVersionTypeFromString(array[0][0].ToString());
            if(!int.TryParse(array[0].Substring(1), out var major))
                return false;
            if (!int.TryParse(array[1], out var minor))
                return false;
            if (!int.TryParse(array[2], out var revision))
                return false;
            int changeSet;
            if (array.Length > 3)
            {
                if (!int.TryParse(array[3], out changeSet))
                    return false;
            }
            else
                changeSet = 224785;
            version = new ApplicationVersion(applicationVersionType, major, minor, revision, changeSet);
            return true;
        }
    }
}