using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.GameFeatures;
using MCM.Internal.Extensions;

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
        private static class PlatformFileHelperUtils
        {
            private delegate TWPlatformFilePath[] V1Delegate(object instance, TWPlatformDirectoryPath path, string searchPattern);
            private delegate TWPlatformFilePath[] V2Delegate(object instance, TWPlatformDirectoryPath path, string searchPattern, int searchOption);

            private static readonly object _sync = new();
            private static Type? CapturedType;
            private static V1Delegate? V1;
            private static V2Delegate? V2;

            private static void ReCheck(Type type)
            {
                if (CapturedType == type) return;

                lock (_sync)
                {
                    if (CapturedType == type) return;

                    V1 = null;
                    V2 = null;

                    foreach (var methodInfo in AccessTools.GetDeclaredMethods(type).Where(x => x.Name == "GetFiles"))
                    {
                        var @params = methodInfo.GetParameters();
                        if (@params.Length == 2)
                            V1 = AccessTools2.GetDelegate<V1Delegate>(methodInfo);
                        if (@params.Length == 3)
                            V2 = AccessTools2.GetDelegate<V2Delegate>(methodInfo);
                    }

                    CapturedType = type;
                }
            }
        
            public static TWPlatformFilePath[] GetFiles(object instance, TWPlatformDirectoryPath path, string searchPattern)
            {
                ReCheck(instance.GetType());
                
                if (V1 is not null)
                    return V1(instance, path, searchPattern);
                if (V2 is not null)
                    return V2(instance, path, searchPattern, 0);
                return [];
            }
        }
        
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
            return PlatformFileHelperUtils.GetFiles(PlatformFileHelper, new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path), searchPattern)
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
                PlatformFileHelper.SaveFile(file, []);
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

        public string? GetSystemPath(GameDirectory directory)
        {
            var baseDirectory = new TWPlatformDirectoryPath((PlatformFileType) directory.Type, directory.Path);
            return PlatformFileHelperPCExtended.GetDirectoryFullPath(baseDirectory);
        }
    }
}