using MBOptionScreen.Interfaces;

namespace MBOptionScreen.Settings
{
    public static class FileDatabase
    {
        private static IFileStorage FileStorage => MBOptionScreenSubModule.SharedStateObject.FileStorage;

        public static T Get<T>(string id) where T : ISerializableFile =>
            FileStorage == null ? default : FileStorage.Get<T>(id);

        public static bool Initialize(string moduleName) =>
            FileStorage?.Initialize(moduleName) == true;

        public static bool SaveToFile(string moduleName, ISerializableFile sf, Location location = Location.Modules) =>
            FileStorage?.SaveToFile(moduleName, sf, location) == true;
    }
}