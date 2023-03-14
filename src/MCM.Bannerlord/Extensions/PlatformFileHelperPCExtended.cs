using HarmonyLib.BUTR.Extensions;

using TaleWorlds.Library;

namespace MCM.Internal.Extensions
{
    // TODO: What to with Xbox version?
    internal static class PlatformFileHelperPCExtended
    {
        private delegate string GetDirectoryFullPathDelegate(object instance, PlatformDirectoryPath directoryPath);
        private static GetDirectoryFullPathDelegate? GetDirectoryFullPathMethod =
            AccessTools2.GetDelegate<GetDirectoryFullPathDelegate>("TaleWorlds.Library.PlatformFileHelperPC:GetDirectoryFullPath");

        private delegate object GetPlatformFileHelperDelegate();
        private static GetPlatformFileHelperDelegate? GetPlatformFileHelper =
            AccessTools2.GetPropertyGetterDelegate<GetPlatformFileHelperDelegate>("TaleWorlds.Library.Common:PlatformFileHelper");

        public static string? GetDirectoryFullPath(PlatformDirectoryPath directoryPath) =>
            GetPlatformFileHelper is not null && GetDirectoryFullPathMethod is not null && GetPlatformFileHelper() is { } obj
                ? GetDirectoryFullPathMethod(obj, directoryPath)
                : null;
    }
}