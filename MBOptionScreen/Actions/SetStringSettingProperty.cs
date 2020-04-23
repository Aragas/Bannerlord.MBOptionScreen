using MBOptionScreen.Settings;

namespace MBOptionScreen.Actions
{
    public sealed class SetStringSettingProperty : IAction
    {
        private readonly string _originalValue;

        public Ref Context { get; }
        public object Value { get; }
        private ISettingPropertyStringValue SettingProperty { get; }

        public SetStringSettingProperty(ISettingPropertyStringValue settingProperty, string value)
        {
            Value = value;
            SettingProperty = settingProperty;
            _originalValue = SettingProperty.StringValue;
        }

        public void DoAction() => SettingProperty.StringValue = (string) Value;
        public void UndoAction() => SettingProperty.StringValue = _originalValue;
    }
}