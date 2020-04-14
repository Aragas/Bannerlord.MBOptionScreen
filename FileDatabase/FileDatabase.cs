using MBOptionScreen;
using ModLib.Interfaces;

namespace ModLib
{
    public static class FileDatabase
    {
        private static IFileStorage FileStorage => MBOptionScreenSubModule.SyncObject.FileStorage;

        public static T Get<T>(string id) where T : ISerialisableFile =>
            FileStorage == null ? default : FileStorage.Get<T>(id);

        public static bool Initialize(string moduleName) =>
            FileStorage?.Initialize(moduleName) == true;

        public static bool SaveToFile(string moduleName, ISerialisableFile sf, Location location = Location.Modules) =>
            FileStorage?.SaveToFile(moduleName, sf, location) == true;
    }
}