using MCM.Abstractions.Settings;

namespace MCM.UI.Actions
{
    internal sealed class SetIntSettingProperty : IAction
    {
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetIntSettingProperty(ISettingPropertyIntValue settingProperty, int value)
        {
            Context = new ProxyRef(() => settingProperty.IntValue, o => settingProperty.IntValue = o as int? ?? 0);
            Value = value;
            Original = settingProperty.IntValue;
        }

        public void DoAction() => Context.Value = (int) Value;
        public void UndoAction() => Context.Value = (int) Original;
    }
}