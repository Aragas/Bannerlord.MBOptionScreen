namespace MCM.Abstractions.GameFeatures
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPathProvider
    {
        public string? GetDocumentsPath();
        public string? GetGamePath();
    }
}