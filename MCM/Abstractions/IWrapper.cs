namespace MCM.Abstractions
{
    public interface IWrapper
    {
        object Object { get; }
        bool IsCorrect { get; }
    }
}