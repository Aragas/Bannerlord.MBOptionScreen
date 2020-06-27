using MCM.Abstractions.Ref;

namespace MCM.UI.Actions
{
    internal interface IAction
    {
        IRef Context { get; }
        object Original { get; }
        object Value { get; }
        void DoAction();
        void UndoAction();
    }
}