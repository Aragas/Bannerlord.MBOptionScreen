using MBOptionScreen.Data;

using TaleWorlds.Core.ViewModelCollection;

namespace MBOptionScreen.Actions
{
    public sealed class SetDropdownAction : IAction
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetDropdownAction(Ref context, SelectorVM<SelectorItemVM> value)
        {
            Context = context;
            Value = value.SelectedIndex;
            Original = (Context.Value as IDropdownProvider)!.SelectedIndex;
        }

        public void DoAction() => (Context!.Value as IDropdownProvider)!.SelectedIndex = (int) Value;
        public void UndoAction() => (Context!.Value as IDropdownProvider)!.SelectedIndex = (int) Original;
    }
}