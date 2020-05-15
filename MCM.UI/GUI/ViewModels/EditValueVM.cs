using MCM.Abstractions.Settings;
using MCM.UI.Actions;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal class EditValueVM : ViewModel
    {
        private string _textInput = "";
        private string _titleText = "title";
        private string _descriptionText = "description";

        public SettingsPropertyVM SettingProperty { get; set; }

        [DataSourceProperty]
        public string TextInput
        {
            get => _textInput;
            set
            {
                if (_textInput != value)
                {
                    _textInput = value;
                    OnPropertyChanged(nameof(TextInput));
                }
            }
        }
        [DataSourceProperty]
        public string TitleText
        {
            get => _titleText;
            set
            {
                if (_titleText != value)
                {
                    _titleText = value;
                    OnPropertyChanged(nameof(TitleText));
                }
            }
        }
        [DataSourceProperty]
        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                if (_descriptionText != value)
                {
                    _descriptionText = value;
                    OnPropertyChanged(nameof(DescriptionText));
                }
            }
        }
        [DataSourceProperty]
        public SettingType SettingType => SettingProperty.SettingType;
        [DataSourceProperty]
        public float MinValue => (float) SettingProperty.SettingPropertyDefinition.EditableMinValue;
        [DataSourceProperty]
        public float MaxValue => (float) SettingProperty.SettingPropertyDefinition.EditableMaxValue;

        public EditValueVM(SettingsPropertyVM settingProperty)
        {
            SettingProperty = settingProperty;

            TitleText = $"Edit \"{SettingProperty.Name}\"";
            switch (SettingType)
            {
                case SettingType.Int:
                case SettingType.Float:
                {
                    var format = SettingProperty.SettingType == SettingType.Int ? "0" : "0.00";
                    DescriptionText = $"Edit the value for \"{SettingProperty.Name}\".\nThe minimum value is {SettingProperty.SettingPropertyDefinition.EditableMinValue.ToString(format)} and the maximum value is {SettingProperty.SettingPropertyDefinition.EditableMaxValue.ToString(format)}.";
                    break;
                }
                case SettingType.String:
                {
                    DescriptionText = $"Edit the value for \"{SettingProperty.Name}\".";
                    break;
                }
            }

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            TextInput = SettingProperty.ValueString ?? "";
        }

        public void ExecuteDone()
        {
            switch (SettingProperty.SettingType)
            {
                case SettingType.Int when int.TryParse(TextInput, out var intVal):
                    SettingProperty.URS.Do(new SetValueTypeAction<int>(SettingProperty.PropertyReference, intVal));
                    break;
                case SettingType.Float when float.TryParse(TextInput, out var floatVal):
                    SettingProperty.URS.Do(new SetValueTypeAction<float>(SettingProperty.PropertyReference, floatVal));
                    break;
                case SettingType.String:
                    SettingProperty.URS.Do(new SetStringAction(SettingProperty.PropertyReference, TextInput));
                    break;
            }

            ScreenManager.PopScreen();
        }

        public void ExecuteCancel() => ScreenManager.PopScreen();
    }
}