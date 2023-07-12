using MCM.Abstractions.GameFeatures;

using System;
using System.Linq;

using TaleWorlds.Engine;
using TaleWorlds.Library;

using TWPlatformDirectoryPath = TaleWorlds.Library.PlatformDirectoryPath;
using TWPlatformFilePath = TaleWorlds.Library.PlatformFilePath;

namespace MCM.Internal.GameFeatures
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class FileSystemProvider : IFileSystemProvider
    {
        private static string EnsureDirectoryEndsInSeparator(string directory) => directory.EndsWith("\\") ? directory : directory + "\\";

        private IPlatformFileHelper PlatformFileHelper => TaleWorlds.Library.Common.PlatformFileHelper;

        private static GameDirectory GetConfigsDirectory()
        {
            var directory = new TWPlatformDirectoryPath(PlatformFileType.User, EnsureDirectoryEndsInSeparator(EngineFilePaths.ConfigsDirectoryName));
            return new GameDirectory((PlatformDirectoryType) directory.Type, directory.Path);
        }

        public GameDirectory GetModSettingsDirectory()
        {
            return GetOrCreateDirectory(GetConfigsDirectory(), EnsureDirectoryEndsInSeparator("ModSettings"));
        }

        public GameDirectory? GetDirectory(GameDirectory directory, string name)
        {
            return directory with { Path = directory.Path + EnsureDirectoryEndsInSeparator(name) };
        }

        public GameDirectory GetOrCreateDirectory(GameDirectory directory, string name)
        {
            return directory with { Path = directory.Path + EnsureDirectoryEndsInSeparator(name) };
        }

        public GameFile[] GetFiles(GameDirectory directory, string searchPattern)
        {
            return PlatformFileHelper.GetFiles(new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path), searchPattern)
                .Select(x => new GameFile(directory, x.FileName))
                .ToArray();
        }

        public GameFile? GetFile(GameDirectory directory, string fileName)
        {
            var file = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path), fileName);
            return !PlatformFileHelper.FileExists(file) ? null : new GameFile(directory, fileName);
        }

        public GameFile GetOrCreateFile(GameDirectory directory, string fileName)
        {
            var file = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path), fileName);
            if (!PlatformFileHelper.FileExists(file))
            {
                PlatformFileHelper.SaveFile(file, Array.Empty<byte>());
            }
            return new GameFile(directory, fileName);
        }

        public bool WriteData(GameFile file, byte[]? data)
        {
            var baseFile = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) file.Owner.Type, file.Owner.Path), file.Name);

            if (data is null)
                return PlatformFileHelper.DeleteFile(baseFile);

            return PlatformFileHelper.SaveFile(baseFile, data) == SaveResult.Success;
        }

        public byte[]? ReadData(GameFile file)
        {
            var baseFile = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) file.Owner.Type, file.Owner.Path), file.Name);
            return !PlatformFileHelper.FileExists(baseFile) ? null : PlatformFileHelper.GetFileContent(baseFile);
        }

        public string? GetSystemPath(GameFile file)
        {
            var baseFile = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) file.Owner.Type, file.Owner.Path), file.Name);
            return PlatformFileHelper.GetFileFullPath(baseFile);
        }
    }
}