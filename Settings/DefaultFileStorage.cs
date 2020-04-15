using MBOptionScreen.Attributes;
using MBOptionScreen.Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace MBOptionScreen.Settings
{
    [FileStorageVersion("e1.0.0",  1)]
    [FileStorageVersion("e1.0.1",  1)]
    [FileStorageVersion("e1.0.2",  1)]
    [FileStorageVersion("e1.0.3",  1)]
    [FileStorageVersion("e1.0.4",  1)]
    [FileStorageVersion("e1.0.5",  1)]
    [FileStorageVersion("e1.0.6",  1)]
    [FileStorageVersion("e1.0.7",  1)]
    [FileStorageVersion("e1.0.8",  1)]
    [FileStorageVersion("e1.0.9",  1)]
    [FileStorageVersion("e1.0.10", 1)]
    [FileStorageVersion("e1.0.11", 1)]
    [FileStorageVersion("e1.1.0",  1)]
    internal class DefaultFileStorage : IFileStorage
    {
        private readonly string LoadablesFolderName = "Loadables";

        public Dictionary<Type, Dictionary<string, ISerializableFile>> Data { get; } = new Dictionary<Type, Dictionary<string, ISerializableFile>>();

        public bool Initialize(string moduleName)
        {
            var successful = false;
            try
            {
                LoadAllFiles(moduleName);
                successful = true;
            }
            catch (Exception ex)
            {
                // TODO
                //ModDebug.ShowError($"An error occurred whilst trying to load files for module: {moduleName}", "Error occurred during loading files", ex);
            }
            return successful;
        }

        public T Get<T>(string id) where T : ISerializableFile
        {
            //First check if the dictionary contains the key
            if (!Data.ContainsKey(typeof(T)))
                return default;

            if (!Data[typeof(T)].ContainsKey(id))
                return default;

            return (T) Data[typeof(T)][id];
        }

        public bool SaveToFile(string moduleName, ISerializableFile sf, Location location = Location.Modules)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sf.Id))
                    throw new Exception($"FileDatabase tried to save an object of type {sf.GetType().FullName} but the ID value was null.");
                if (string.IsNullOrWhiteSpace(moduleName))
                    throw new Exception($"FileDatabase tried to save an object of type {sf.GetType().FullName} with ID {sf.Id} but the module folder name given was null or empty.");

                var path = GetPathForModule(moduleName, location);
                if (!Directory.Exists(path))
                    throw new Exception($"FileDatabase cannot find the module named {moduleName}");

                if (location == Location.Modules)
                    path = System.IO.Path.Combine(path, "ModuleData", LoadablesFolderName);

                if (sf is ISubFolder subFolder && !string.IsNullOrWhiteSpace(subFolder.SubFolder))
                    path = System.IO.Path.Combine(path, subFolder.SubFolder);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = System.IO.Path.Combine(path, $"{sf.GetType().Name}.{sf.Id}.xml");

                if (File.Exists(path))
                    File.Delete(path);

                using var writer = XmlWriter.Create(path, new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true
                });
                var rootNode = new XmlRootAttribute
                {
                    ElementName = $"{sf.GetType().Assembly.GetName().Name}-{sf.GetType().FullName}"
                };
                var xmlns = new XmlSerializerNamespaces(new[]
                {
                    XmlQualifiedName.Empty
                });
                var serializer = new XmlSerializer(sf.GetType(), rootNode);
                serializer.Serialize(writer, sf, xmlns);
                return true;
            }
            catch (Exception ex)
            {
                // TODO
                //ModDebug.ShowError($"Cannot create the file for type {sf.GetType().FullName} with ID {sf.ID} for module {moduleName}:", "Error saving to file", ex);
                return false;
            }
        }

        private void Add(ISerializableFile loadable)
        {
            if (loadable == null)
                throw new ArgumentNullException("Tried to add something to the Loader Data dictionary that was null");
            if (string.IsNullOrWhiteSpace(loadable.Id))
                throw new ArgumentNullException($"Loadable of type {loadable.GetType().ToString()} has missing ID field");

            var type = loadable.GetType();
            if (!Data.ContainsKey(type))
                Data.Add(type, new Dictionary<string, ISerializableFile>());

            if (Data[type].ContainsKey(loadable.Id))
            {
                // TODO
                //ModDebug.LogError($"Loader already contains Type: {type.AssemblyQualifiedName} ID: {loadable.ID}, overwriting...");
                Data[type][loadable.Id] = loadable;
            }
            else
                Data[type].Add(loadable.Id, loadable);
        }

        private void LoadFromFile(string filePath)
        {
            //DEBUG:: People can't read and aren't deleting the old mod installation. Need to manually delete the old config file for a couple updates.
            var modulefolder = Directory.GetParent(filePath).Parent.Parent.Name;
            if (System.IO.Path.GetFileName(filePath) == "config.xml" && modulefolder == "zzBannerlordTweaks")
            {
                File.Delete(filePath);
                return;
            }

            using var reader = XmlReader.Create(filePath);
            var nodeData = "";
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

                var root = new XmlRootAttribute
                {
                    ElementName = nodeData,
                    IsNullable = true
                };
                var serializer = new XmlSerializer(data.Type, root);
                var loaded = (ISerializableFile) serializer.Deserialize(reader);
                if (loaded != null)
                    Add(loaded);
                else
                    throw new Exception($"Unable to load {data.FullName} from file {filePath}.");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException exception && exception.ParamName == "type")
                    throw new Exception($"Cannot get a type from type name {nodeData} in file {filePath}", exception);

                throw new Exception($"An error occurred whilst loading file {filePath}", ex);
            }
        }

        /// <summary>
        /// Loads all files in the Loadables folder for the given module and from the Documents folder for the given module.
        /// </summary>
        /// <param name="moduleName">This is the name of the module to load the files for. This is the name of the module folder.</param>
        private void LoadAllFiles(string moduleName)
        {
            #region Loadables Folder
            //Check if the given module name is correct
            var modulePath = GetPathForModule(moduleName, Location.Modules);
            if (!Directory.Exists(modulePath))
                throw new Exception($"Cannot find module named {moduleName}");
            //Check the module's ModuleData folder for the Loadables folder.
            var moduleLoadablesPath = System.IO.Path.Combine(modulePath, "ModuleData", LoadablesFolderName);
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
                    foreach (var dir in subDirs)
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
                                    // TODO
                                    //ModDebug.LogError($"Failed to load file: {filePath} \n\nSkipping..\n\n", ex);
                                }
                            }
                        }

                        break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while FileDatabase was trying to load all files for module {moduleName}", ex);
                }
            }
            else
                Directory.CreateDirectory(moduleLoadablesPath);
            #endregion
            #region Documents Folder
            //TODO::
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
                        // TODO
                        //ModDebug.LogError($"Failed to load file: {filePath}\n\n Skipping...", ex);
                    }
                }
            }
            else
                Directory.CreateDirectory(modConfigsPath);
            #endregion
        }

        private static string GetPathForModule(string moduleName, Location location) => location switch
        {
            Location.Modules => System.IO.Path.Combine(BasePath.Name, "Modules", moduleName),
            Location.Configs => System.IO.Path.Combine(Utilities.GetConfigsPath(), moduleName)
        };

        private class TypeData
        {
            public string AssemblyName { get; }
            public string TypeName { get; }
            public string FullName => $"{TypeName}, {AssemblyName}";
            public Type Type => Type.GetType(FullName);

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
                    throw new ArgumentException($"The given node data was invalid.\nNode Data: {nodeData}");
            }
        }
    }
}