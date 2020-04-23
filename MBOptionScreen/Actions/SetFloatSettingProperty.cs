using MBOptionScreen.Settings;

namespace MBOptionScreen.Actions
{
    public sealed class SetFloatSettingProperty : IAction
    {
        private readonly float _originalValue;

        public Ref Context { get; }
        public object Value { get; }
        private ISettingPropertyFloatValue SettingProperty { get; }

        public SetFloatSettingProperty(ISettingPropertyFloatValue settingProperty, float value)
        {
            Value = value;
            SettingProperty = settingProperty;
            _originalValue = SettingProperty.FloatValue;
        }

        public void DoAction() => SettingProperty.FloatValue = (float)Value;
        public void UndoAction() => SettingProperty.FloatValue = _originalValue;
    }
}