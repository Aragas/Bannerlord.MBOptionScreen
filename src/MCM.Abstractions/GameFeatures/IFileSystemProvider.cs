namespace MCM.Abstractions.GameFeatures
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    enum PlatformDirectoryType
    {
        User,
        Application,
        Temporary,
    }
    
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        record GameDirectory(PlatformDirectoryType Type, string Path);
    
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        record GameFile(GameDirectory Owner, string Name);


#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        interface IFileSystemProvider
    {
        GameDirectory GetModSettingsDirectory();
        GameDirectory? GetDirectory(GameDirectory directory, string directoryName);
        GameDirectory GetOrCreateDirectory(GameDirectory rootFolder, string id);
        GameFile[] GetFiles(GameDirectory directory, string searchPattern);
        GameFile? GetFile(GameDirectory directory, string fileName);
        GameFile GetOrCreateFile(GameDirectory directory, string fileName);
        bool WriteData(GameFile file, byte[] data);
        byte[]? ReadData(GameFile file);
    }
}