using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.GameFeatures;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace MCM.Internal.GameFeatures
{
    internal sealed class PathProvider : IPathProvider
    {
        private delegate string GetDirectoryFullPathDelegate(PlatformFileHelperPC instance, PlatformDirectoryPath directoryPath);
        private static readonly GetDirectoryFullPathDelegate GetDirectoryFullPath =
            AccessTools2.GetDelegate<GetDirectoryFullPathDelegate>("TaleWorlds.Library.PlatformFileHelperPC:GetDirectoryFullPath");

        /// <inheritdoc />
        public string? GetDocumentsPath() => TaleWorlds.Library.Common.PlatformFileHelper is PlatformFileHelperPC instance
            ? GetDirectoryFullPath(instance, new PlatformDirectoryPath(PlatformFileType.User, string.Empty))
            : null;

        /// <inheritdoc />
        public string? GetGamePath() => Utilities.GetBasePath();
    }
}