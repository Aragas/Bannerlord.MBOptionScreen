namespace MCM.Abstractions.GameFeatures
{
    public interface IPathProvider
    {
        public string? GetDocumentsPath();
        public string? GetGamePath();
    }
}