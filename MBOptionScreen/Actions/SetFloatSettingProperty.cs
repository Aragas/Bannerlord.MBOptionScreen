using MBOptionScreen.Settings;

namespace MBOptionScreen.Actions
{
    public sealed class SetFloatSettingProperty : IAction
    {
        public Ref? Context { get; }
        public object Value { get; }
        public object Original { get; }
        private ISettingPropertyFloatValue SettingProperty { get; }

        public SetFloatSettingProperty(ISettingPropertyFloatValue settingProperty, float value)
        {
            Value = value;
            SettingProperty = settingProperty;
            Original = SettingProperty.FloatValue;
        }

        public void DoAction() => SettingProperty.FloatValue = (float) Value;
        public void UndoAction() => SettingProperty.FloatValue = (float) Original;
    }
}