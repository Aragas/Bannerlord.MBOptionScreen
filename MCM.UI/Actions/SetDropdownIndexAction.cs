using HarmonyLib;

using System.Reflection;

using TaleWorlds.Core.ViewModelCollection;

namespace MCM.UI.Actions
{
    internal sealed class SetDropdownIndexAction : IAction
    {
        private readonly PropertyInfo SelectedIndexProperty;
        private static int GetSelectedIndex(object dropdown)
        {
            var selectedIndexProperty = AccessTools.Property(dropdown.GetType(), "SelectedIndex");
            if (selectedIndexProperty == null)
                return 0;
            return (int) selectedIndexProperty.GetValue(dropdown);
        }

        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetDropdownIndexAction(Ref context, SelectorVM<SelectorItemVM> value)
        {
            SelectedIndexProperty = AccessTools.Property(context.Value.GetType(), "SelectedIndex");

            Context = context;
            Value = value.SelectedIndex;
            Original = SelectedIndexProperty?.GetValue(Context.Value) ?? 0;
        }

        public void DoAction() => SelectedIndexProperty?.SetValue(Context!.Value, Value);
        public void UndoAction() => SelectedIndexProperty?.SetValue(Context!.Value, Original);
    }
}