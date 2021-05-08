extern alias v13;

using Bannerlord.BUTR.Shared.Helpers;

using ModLib.Debugging;
using ModLib.Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ModLib
{
    public static class FileDatabase
    {
        private const string LoadablesFolderName = "Loadables";
        public static Dictionary<Type, Dictionary<string, ISerialisableFile>> Data { get; } = new();

        /// <summary>
        /// Returns the ISerialisableFile of type T with the given ID from the database. If it cannot be found, returns null.
        /// </summary>
        /// <typeparam name="T">Type of object to retrieve.</typeparam>
        /// <param name="id">ID of object to retrieve.</param>
        /// <returns>Returns the instance of the object with the given type and ID. If it cannot be found, returns null.</returns>
        public static T? Get<T>(string id) where T : class
        {
            // Some new ModLib mods love to use FileDabatabse.Get instead of SettingsDatabase.Get
            if (typeof(v13::ModLib.Definitions.Interfaces.ISerialisableFile).IsAssignableFrom(typeof(T)))
                return v13::ModLib.Definition.FileDatabaseWorkaround.Get<T>(id);


            //First check if the dictionary contains the key
            if (!Data.ContainsKey(typeof(T)))
                return default;
            if (!Data[typeof(T)].ContainsKey(id))
                return default;

            return Data[typeof(T)][id] as T;
        }

        /// <summary>
        /// Loads all files for the given module.
        /// </summary>
        /// <param name="moduleFolderName">Name of the module to load the files from. This is the name of the actual folder in the Bannerlord Modules folder.</param>
        /// <returns>Returns true if initialisation was successful.</returns>
        public static bool Initialise(string moduleFolderName)
        {
            var successful = false;
            try
            {
                LoadAllFiles(moduleFolderName);
                successful = true;
            }
            catch (Exception ex)
            {
                ModDebug.ShowError($"An error occurred whilst trying to load files for module: {moduleFolderName}", "Error occurred during loading files", ex);
            }
            return successful;
        }

        /// <summary>
        /// Saves the given object instance which inherits ISerialisableFile to an xml file.
        /// </summary>
        /// <param name="moduleFolderName">The folder name of the module to save to.</param>
        /// <param name="sf">Instance of the object to save to file.</param>
        /// <param name="location">Indicates whether to save the file to the ModuleData/Loadables folder or to the mod's Config folder in Bannerlord's 'My Documents' directory.</param>
        public static bool SaveToFile(string moduleFolderName, ISerialisableFile? sf, Location location = Location.Modules)
        {
            try
            {
                if (sf == null) throw new ArgumentNullException(nameof(sf));
                if (string.IsNullOrWhiteSpace(sf.ID))
                    throw new Exception($"FileDatabase tried to save an object of type {sf.GetType().FullName} but the ID value was null.");
                if (string.IsNullOrWhiteSpace(moduleFolderName))
                    throw new Exception($"FileDatabase tried to save an object of type {sf.GetType().FullName} with ID {sf.ID} but the module folder name given was null or empty.");

                //Gets the intended path for the file.
                var path = GetPathForModule(moduleFolderName, location);

                if (location == Location.Modules && !Directory.Exists(path))
                    throw new Exception($"FileDatabase cannot find the module named {moduleFolderName}");
                else if (location == Location.Configs && !Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (location == Location.Modules)
                    path = Path.Combine(path, "ModuleData", LoadablesFolderName);

                if (sf is ISubFolder subFolder)
                {
                    if (!string.IsNullOrWhiteSpace(subFolder.SubFolder))
                        path = Path.Combine(path, subFolder.SubFolder);
                }

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, GetFileNameFor(sf));

                if (File.Exists(path))
                    File.Delete(path);

                using (var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
                {
                    var rootNode = new XmlRootAttribute { ElementName = $"{sf.GetType().Assembly.GetName().Name}-{sf.GetType().FullName}" };
                    var xmlns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var serializer = new XmlSerializer(sf.GetType(), rootNode);
                    serializer.Serialize(writer, sf, xmlns);
                }
                return true;
            }
            catch (Exception ex)
            {
                ModDebug.ShowError($"Cannot create the file for type {sf?.GetType().FullName} with ID {sf?.ID} for module {moduleFolderName}:", "Error saving to file", ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes the file for the given fileName and module.
        /// </summary>
        /// <param name="moduleFolderName">The folder name of the module to delete the file for.</param>
        /// <param name="fileName">The file name of the file to be deleted.</param>
        /// <param name="location">The location of the file to be deleted.</param>
        /// <returns>Returns true if the file was deleted successfully.</returns>
        public static bool DeleteFile(string moduleFolderName, string fileName, Location location = Location.Modules)
        {
            var successful = true;
            var path = GetPathForModule(moduleFolderName, location);
            if (!Directory.Exists(path))
            {
                ModDebug.ShowError($"Tried to delete a file with file name {fileName} from directory \"{path}\" but the directory doesn't exist.", "Could not find directory");
                successful = false;
            }

            if (location == Location.Modules)
                path = Path.Combine(path, "ModuleData", LoadablesFolderName);

            var filePath = Path.Combine(path, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            return successful;
        }

        /// <summary>
        /// Deletes the file for the given object instance and module.
        /// </summary>
        /// <param name="moduleFolderName">The folder name of the module to delete the file for.</param>
        /// <param name="sf">The instance of the object whose file should be deleted.</param>
        /// <param name="location">The location of the file to be deleted.</param>
        /// <returns>Returns true if the file was deleted successfully.</returns>
        public static bool DeleteFile(string moduleFolderName, ISerialisableFile sf, Location location = Location.Modules) =>
            DeleteFile(moduleFolderName, GetFileNameFor(sf), location);

        private static void Add(ISerialisableFile loadable)
        {
            if (loadable == null)
                throw new ArgumentNullException(nameof(loadable), "Tried to add something to the FileDatabase Data dictionary that was null");
            if (string.IsNullOrWhiteSpace(loadable.ID))
                throw new ArgumentNullException($"Loadable of type {loadable.GetType()} has missing ID field");

            var type = loadable.GetType();
            if (!Data.ContainsKey(type))
                Data.Add(type, new Dictionary<string, ISerialisableFile>());

            if (Data[type].ContainsKey(loadable.ID))
            {
                ModDebug.LogError($"Loader already contains Type: {type.AssemblyQualifiedName} ID: {loadable.ID}, overwriting...");
                Data[type][loadable.ID] = loadable;
            }
            else
            {
                Data[type].Add(loadable.ID, loadable);
            }
        }

        private static void LoadFromFile(string filePath)
        {
            using (var reader = XmlReader.Create(filePath))
            {
                var nodeData = string.Empty;
                try
                {
                    //Find the type name
                    if (reader.MoveToContent() == XmlNodeType.Element)
                        nodeData = reader.Name;
                    //If we couldn't find the type name, throw an exception saying so. If the root node doesn't include the namespace, throw an exception saying so.
                    if (string.IsNullOrWhiteSpace(nodeData))
                        throw new Exception($"Could not find the root node in xml document located at {filePath}");

                    var data = new TypeData(nodeData);
                    //Find the type from the root node name. The root node should be the full name of the type, including the namespace and the assembly.

                    if (data.Type == null)
                        throw new Exception($"Unable to find type {data.FullName}");

                    var root = new XmlRootAttribute { ElementName = nodeData, IsNullable = true };
                    var serialiser = new XmlSerializer(data.Type, root);
                    if (serialiser.Deserialize(reader) is ISerialisableFile loaded)
                        Add(loaded);
                    else
                        throw new Exception($"Unable to load {data.FullName} from file {filePath}.");
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException { ParamName: "type" } argumentNullException)
                        throw new Exception($"Cannot get a type from type name {nodeData} in file {filePath}", argumentNullException);
                    throw new Exception($"An error occurred whilst loading file {filePath}", ex);
                }
            }
        }

        /// <summary>
        /// Loads all files in the Loadables folder for the given module and from the Documents folder for the given module.
        /// </summary>
        /// <param name="moduleName">This is the name of the module to load the files for. This is the name of the module folder.</param>
        private static void LoadAllFiles(string moduleName)
        {
            #region Loadables Folder
            //Check if the given module name is correct
            var modulePath = GetPathForModule(moduleName, Location.Modules);
            if (!Directory.Exists(modulePath))
                throw new Exception($"Cannot find module named {moduleName}");
            //Check the module's ModuleData folder for the Loadables folder.
            var moduleLoadablesPath = Path.Combine(modulePath, "ModuleData", LoadablesFolderName);
            if (Directory.Exists(moduleLoadablesPath))
            {
                try
                {
                    //If the module has a Loadables folder, loop through it and load all the files.
                    //Starting with the files in the root folder
                    foreach (var filePath in Directory.GetFiles(moduleLoadablesPath, "*.xml"))
                    {
                        LoadFromFile(filePath);
                    }

                    //Loop through any subfolders and load the files in them
                    var subDirs = Directory.GetDirectories(moduleLoadablesPath);
                    if (subDirs.Length > 0)
                    {
                        foreach (var subDir in subDirs)
                        {
                            foreach (var filePath in Directory.GetFiles(subDir, "*.xml"))
                            {
                                try
                                {
                                    LoadFromFile(filePath);
                                }
                                catch (Exception ex)
                                {
                                    ModDebug.LogError($"Failed to load file: {filePath} \n\nSkipping..\n\n", ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while FileDatabase was trying to load all files for module {moduleName}", ex);
                }
            }
            else
            {
                Directory.CreateDirectory(moduleLoadablesPath);
            }
            #endregion
            #region Documents Folder
            var modConfigsPath = GetPathForModule(moduleName, Location.Configs);
            if (Directory.Exists(modConfigsPath))
            {
                foreach (var filePath in Directory.GetFiles(modConfigsPath))
                {
                    try
                    {
                        LoadFromFile(filePath);
                    }
                    catch (Exception ex)
                    {
                        ModDebug.LogError($"Failed to load file: {filePath}\n\n Skipping...", ex);
                    }
                }
                var subfolders = Directory.GetDirectories(modConfigsPath);
                if (subfolders.Length > 0)
                {
                    foreach (var subFolder in subfolders)
                    {
                        foreach (var filePath in Directory.GetFiles(subFolder))
                        {
                            try
                            {
                                LoadFromFile(filePath);
                            }
                            catch (Exception ex)
                            {
                                ModDebug.LogError($"Failed to load file: {filePath}\n\n Skipping...", ex);
                            }
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(modConfigsPath);
            }

            #endregion
        }

        /// <summary>
        /// Returns the file name for the given ISerialisableFile
        /// </summary>
        /// <param name="sf">The instance of the ISerialisableFile to retrieve the file name for.</param>
        /// <returns>Returns the file name of the given ISerialisableFile, including the file extension.</returns>
        public static string GetFileNameFor(ISerialisableFile sf)
        {
            if (sf == null) throw new ArgumentNullException(nameof(sf));
            return $"{sf.GetType().Name}.{sf.ID}.xml";
        }

        /// <summary>
        /// Returns the root folder for the given module name and intended location.
        /// </summary>
        /// <param name="moduleFolderName">Name of the Module's Folder.</param>
        /// <param name="location">Which location to get the path to - configs or the mod's module folder.</param>
        /// <returns></returns>
        public static string GetPathForModule(string moduleFolderName, Location location) => location == Location.Modules
            ? Path.Combine(TaleWorlds.Library.BasePath.Name, "Modules", moduleFolderName)
            : Path.Combine(FSIOHelper.GetConfigPath(), moduleFolderName);

        private class TypeData
        {
            public string AssemblyName { get; }
            public string TypeName { get; }
            public string FullName => $"{TypeName}, {AssemblyName}";
            private Type? _type;
            public Type? Type => _type ??= Array.Find(AppDomain.CurrentDomain.GetAssemblies(), z => z.FullName?.StartsWith(AssemblyName) == true)?.GetType(TypeName);

            public TypeData(string nodeData)
            {
                if (!string.IsNullOrWhiteSpace(nodeData))
                {
                    if (!nodeData.Contains("-"))
                        throw new ArgumentException($"Node data does not contain an assembly string\nNode Data: {nodeData}");
                    if (!nodeData.Contains("."))
                        throw new ArgumentException($"Node data does not contain a namespace string\nNode Data: {nodeData}");

                    var split = nodeData.Split('-');

                    if (!string.IsNullOrWhiteSpace(split[0]))
                        AssemblyName = split[0];
                    else
                        throw new ArgumentException($"Assembly name in node data was null or empty\nNode Data: {nodeData}");

                    if (!string.IsNullOrWhiteSpace(split[1]))
                        TypeName = split[1];
                    else
                        throw new ArgumentException($"Type name in node data was null or empty\nNode Data: {nodeData}");
                }
                else
                {
                    throw new ArgumentException($"The given node data was invalid.\nNode Data: {nodeData}");
                }
            }
        }

        public enum Location
        {
            Modules,
            Configs
        }
    }
}