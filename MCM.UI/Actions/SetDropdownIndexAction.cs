using HarmonyLib;

using MCM.Abstractions.Data;
using MCM.Abstractions.Ref;

using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.UI.Actions
{
    internal sealed class SetDropdownIndexAction : IAction
    {
        private PropertyInfo SelectedIndexProperty { get; }
        private IRef DropdownContext { get; }
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetDropdownIndexAction(IRef context, SelectorVM<SelectorItemVM> value)
        {
            DropdownContext = context;
            SelectedIndexProperty = AccessTools.Property(DropdownContext.Value.GetType(), nameof(IDropdownProvider.SelectedIndex));
            Context = new ProxyRef<object>(() => SelectedIndexProperty.GetValue(DropdownContext.Value), o => SelectedIndexProperty.SetValue(DropdownContext.Value, o));
            Value = value.SelectedIndex;
            Original = Context.Value;
        }

        public void DoAction() => Context.Value = Value;
        public void UndoAction() => Context.Value = Original;
    }
}