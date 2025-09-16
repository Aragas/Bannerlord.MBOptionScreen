using MCM.Abstractions.GameFeatures;
using MCM.Internal.Extensions;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using TaleWorlds.Engine;
using TaleWorlds.Library;

using Path = System.IO.Path;
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
            var path = PlatformFileHelperPCExtended.GetDirectoryFullPath(new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path));

            try
            {
                if (!Directory.Exists(path))
                    return [];
                
                return Directory.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly)
                    .Select(x => new GameFile(directory, Path.GetFileName(x)))
                    .ToArray();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return [];
            }
        }

        public GameFile? GetFile(GameDirectory directory, string fileName)
        {
            var file = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path), fileName).FileFullPath;
            return File.Exists(file) ? new GameFile(directory, fileName) : null;
        }

        public GameFile GetOrCreateFile(GameDirectory directory, string fileName)
        {
            var file = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path), fileName).FileFullPath;
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            return new GameFile(directory, fileName);
        }

        public bool WriteData(GameFile file, byte[]? data)
        {
            var baseFile = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) file.Owner.Type, file.Owner.Path), file.Name).FileFullPath;

            try
            {
                if (data is null)
                {
                    File.Delete(baseFile);
                }
                else
                {
                    File.WriteAllBytes(baseFile, data);
                }
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return false;
            }
        }

        public byte[]? ReadData(GameFile file)
        {
            var baseFile = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) file.Owner.Type, file.Owner.Path), file.Name).FileFullPath;

            try
            {
                if (!File.Exists(baseFile))
                {
                    return null;
                }
                
                return File.ReadAllBytes(baseFile);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return null;
            }
        }

        public string? GetSystemPath(GameFile file)
        {
            var baseFile = new TWPlatformFilePath(new TWPlatformDirectoryPath((PlatformFileType) file.Owner.Type, file.Owner.Path), file.Name).FileFullPath;
            return baseFile;
        }

        public string? GetSystemPath(GameDirectory directory)
        {
            var baseDirectory = new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path);
            return PlatformFileHelperPCExtended.GetDirectoryFullPath(baseDirectory);
        }
    }
}