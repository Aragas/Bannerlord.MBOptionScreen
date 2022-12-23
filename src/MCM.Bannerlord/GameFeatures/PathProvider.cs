using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.GameFeatures;

using TaleWorlds.Engine;

namespace MCM.Internal.GameFeatures
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class PathProvider : IPathProvider
    {
        /// <inheritdoc />
        public string? GetDocumentsPath() => PlatformFileHelperPCExtended.GetDirectoryFullPath(EngineFilePaths.ConfigsPath);

        /// <inheritdoc />
        public string? GetGamePath() => Utilities.GetBasePath();
    }
}