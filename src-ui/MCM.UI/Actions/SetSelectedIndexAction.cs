using MCM.Common;

namespace MCM.UI.Actions;

internal sealed class SetSelectedIndexAction : IAction
{
    public IRef Context { get; }
    public object? Value { get; }
    public object? Original { get; }

    public SetSelectedIndexAction(IRef context, object value)
    {
        Context = context;
        Value = new SelectedIndexWrapper(value).SelectedIndex;
        Original = new SelectedIndexWrapper(context.Value).SelectedIndex;
    }

    public void DoAction() => new SelectedIndexWrapper(Context.Value) { SelectedIndex = (int?) Value ?? -1 };
    public void UndoAction() => new SelectedIndexWrapper(Context.Value) { SelectedIndex = (int?) Original ?? -1 };
}