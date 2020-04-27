using MBOptionScreen.Settings;

namespace MBOptionScreen.Actions
{
    public sealed class SetStringSettingProperty : IAction
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }
        private ISettingPropertyStringValue SettingProperty { get; }

        public SetStringSettingProperty(ISettingPropertyStringValue settingProperty, string value)
        {
            Value = value;
            SettingProperty = settingProperty;
            Original = SettingProperty.StringValue;
        }

        public void DoAction() => SettingProperty.StringValue = (string) Value;
        public void UndoAction() => SettingProperty.StringValue = (string) Original;
    }
}