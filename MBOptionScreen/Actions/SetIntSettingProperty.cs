using MBOptionScreen.Settings;

namespace MBOptionScreen.Actions
{
    public sealed class SetIntSettingProperty : IAction
    {
        private readonly int _originalValue;

        public Ref Context { get; }
        public object Value { get; }
        private ISettingPropertyIntValue SettingProperty { get; }

        public SetIntSettingProperty(ISettingPropertyIntValue settingProperty, int value)
        {
            Value = value;
            SettingProperty = settingProperty;
            _originalValue = SettingProperty.IntValue;
        }

        public void DoAction() => SettingProperty.IntValue = (int)Value;
        public void UndoAction() => SettingProperty.IntValue = _originalValue;
    }
}