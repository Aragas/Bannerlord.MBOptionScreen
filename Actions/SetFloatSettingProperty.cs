using MBOptionScreen.GUI.v1a.ViewModels;
using MBOptionScreen.Interfaces;

namespace MBOptionScreen.Actions
{
    public class SetFloatSettingProperty : IAction
    {
        private readonly float _originalValue;

        public Ref Context { get; } = null;
        public object Value { get; }
        private SettingPropertyVM SettingProperty { get; }

        public SetFloatSettingProperty(SettingPropertyVM settingProperty, float value)
        {
            Value = value;
            SettingProperty = settingProperty;
            _originalValue = SettingProperty.FloatValue;
        }

        public void DoAction() => SettingProperty.FloatValue = (float)Value;
        public void UndoAction() => SettingProperty.FloatValue = _originalValue;
    }
}
