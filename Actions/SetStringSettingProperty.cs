using MBOptionScreen.GUI.v1a.ViewModels;
using MBOptionScreen.Interfaces;

namespace MBOptionScreen.Actions
{
    public class SetStringSettingProperty : IAction
    {
        private readonly string _originalValue;

        public Ref Context { get; } = null;
        public object Value { get; }
        private SettingProperty SettingProperty { get; }

        public SetStringSettingProperty(SettingProperty settingProperty, string value)
        {
            Value = value;
            SettingProperty = settingProperty;
            _originalValue = SettingProperty.StringValue;
        }

        public void DoAction() => SettingProperty.StringValue = (string) Value;
        public void UndoAction() => SettingProperty.StringValue = _originalValue;
    }
}