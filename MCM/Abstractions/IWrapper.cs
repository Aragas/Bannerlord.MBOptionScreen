namespace MCM.Abstractions
{
    public interface IDependency
    {

    }

    public interface IDependencyBase
    {

    }

    public interface IWrapper
    {
        object Object { get; }
        bool IsCorrect { get; }
    }
}