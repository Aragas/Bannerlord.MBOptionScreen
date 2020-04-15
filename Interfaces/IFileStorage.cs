using MBOptionScreen.Settings;

namespace MBOptionScreen.Interfaces
{
    public interface IFileStorage
    {
        /// <summary>
        /// Loads all files for the given module.
        /// </summary>
        /// <param name="moduleName">Name of the module to load the files from. This is the name of the actual folder in the Bannerlord Modules folder.</param>
        /// <returns>Returns true if initialisation was successful.</returns>
        bool Initialize(string moduleName);

        /// <summary>
        /// Returns the ILoadable of type T with the given ID.
        /// </summary>
        /// <typeparam name="T">Type of object to retrieve</typeparam>
        /// <param name="id">ID of object to retrieve</param>
        /// <returns></returns>
        T Get<T>(string id) where T : ISerializableFile;

        /// <summary>
        /// Saves the given instance to file.
        /// </summary>
        /// <typeparam name="T">Type of the instance to save to file.</typeparam>
        /// <param name="moduleName">The folder name of the module to save to.</param>
        /// <param name="sf">Instance of the object to save to file.</param>
        bool SaveToFile(string moduleName, ISerializableFile sf, Location location = Location.Modules);

    }
}