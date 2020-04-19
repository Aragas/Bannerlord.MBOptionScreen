using MBOptionScreen.Actions;

namespace MBOptionScreen.Interfaces
{
    public interface IAction
    {
        Ref Context { get; }
        object Value { get; }
        void DoAction();
        void UndoAction();
    }
}
