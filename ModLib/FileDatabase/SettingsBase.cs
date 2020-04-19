using ModLib.Interfaces;

namespace ModLib
{
    public abstract class SettingsBase : ISerialisableFile, ISubFolder
    {
        /// <summary>
        /// Unique identifier used to store the settings instance in the settings database and to save to file. Make sure this is unique to your mod.
        /// </summary>
        public abstract string ID { get; set; }
        /// <summary>
        /// The folder name of your mod's 'Modules' folder. Should be identical.
        /// </summary>
        public abstract string ModuleFolderName { get; }
        /// <summary>
        /// The name of your mod. This is used in the mods list in the settings menu.
        /// </summary>
        public abstract string ModName { get; }
        /// <summary>
        /// If you want this settings file stored inside a subfolder, set this to the name of the subfolder.
        /// </summary>
        public virtual string SubFolder => "";
    }
}