namespace MBOptionScreen.Actions
{
    public interface IAction
    {
        Ref? Context { get; }
        object Original { get; }
        object Value { get; }
        void DoAction();
        void UndoAction();
    }
}