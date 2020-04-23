namespace MBOptionScreen.ApplicationContainer
{
    public interface IApplicationContainerProvider
    {
        object Get(string name);
        void Set(string name, object value);

        void Clear();
    }
}