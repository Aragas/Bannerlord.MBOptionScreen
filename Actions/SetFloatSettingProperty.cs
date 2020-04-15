using MBOptionScreen.GUI.v1a.ViewModels;
using MBOptionScreen.Interfaces;

namespace MBOptionScreen.Actions
{
    public class SetFloatSettingProperty : IAction
    {
        private readonly float _originalValue;

        public Ref Context { get; } = null;
        public object Value { get; }
        private SettingProperty SettingProperty { get; }

        public SetFloatSettingProperty(SettingProperty settingProperty, float value)
        {
            Value = value;
            SettingProperty = settingProperty;
            _originalValue = SettingProperty.IntValue;
        }

        public void Do() => SettingProperty.FloatValue = (float)Value;
        public void Undo() => SettingProperty.FloatValue = _originalValue;
    }
}
