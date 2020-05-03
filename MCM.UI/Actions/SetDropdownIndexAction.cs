using HarmonyLib;

using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.UI.Actions
{
    internal sealed class SetDropdownIndexAction : IAction
    {
        private IRef DropdownContext { get; }
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetDropdownIndexAction(IRef context, SelectorVM<SelectorItemVM> value)
        {
            var selectedIndexProperty = AccessTools.Property(context.Value.GetType(), "SelectedIndex");

            DropdownContext = context;
            Context = new ProxyRef(() => selectedIndexProperty.GetValue(DropdownContext.Value), o => selectedIndexProperty.SetValue(DropdownContext.Value, o));
            Value = value.SelectedIndex;
            Original = selectedIndexProperty.GetValue(Context.Value);
        }

        public void DoAction() => Context!.Value = Value;
        public void UndoAction() => Context!.Value = Original;
    }
}