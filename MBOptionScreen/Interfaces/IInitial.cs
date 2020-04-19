using MBOptionScreen.Actions;

namespace MBOptionScreen.Interfaces
{
    public interface IInitial
    {
        Ref Context { get; }
        object Value { get; }
        void Reset();
        bool Changed();
    }
}
