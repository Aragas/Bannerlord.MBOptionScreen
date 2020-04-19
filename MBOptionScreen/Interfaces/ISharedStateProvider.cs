namespace MBOptionScreen.Interfaces
{
    public interface IStateProvider
    {
        T Get<T>() where T : ISharedStateObject;
        void Set<T>(T value) where T : ISharedStateObject;

        void Clear();
    }
}