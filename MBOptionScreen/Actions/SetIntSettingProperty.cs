using MBOptionScreen.Settings;

namespace MBOptionScreen.Actions
{
    public sealed class SetIntSettingProperty : IAction
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }
        private ISettingPropertyIntValue SettingProperty { get; }

        public SetIntSettingProperty(ISettingPropertyIntValue settingProperty, int value)
        {
            Value = value;
            SettingProperty = settingProperty;
            Original = SettingProperty.IntValue;
        }

        public void DoAction() => SettingProperty.IntValue = (int) Value;
        public void UndoAction() => SettingProperty.IntValue = (int) Original;
    }
}