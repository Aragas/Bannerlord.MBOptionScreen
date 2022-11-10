using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.GameFeatures;

using TaleWorlds.Engine;

namespace MCM.Internal.GameFeatures
{
    internal sealed class PathProvider : IPathProvider
    {
        /// <inheritdoc />
        public string? GetDocumentsPath() => PlatformFileHelperPCExtended.GetDirectoryFullPath(EngineFilePaths.ConfigsPath);

        /// <inheritdoc />
        public string? GetGamePath() => Utilities.GetBasePath();
    }
}