namespace MCM.UI.Actions
{
    internal interface IAction
    {
        Ref? Context { get; }
        object Original { get; }
        object Value { get; }
        void DoAction();
        void UndoAction();
    }
}