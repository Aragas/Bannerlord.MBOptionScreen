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

        public SettingPropertyVM SettingProperty { get; set; }

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
                _titleText = value;
                OnPropertyChanged(nameof(TitleText));
            }
        }
        [DataSourceProperty]
        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                _descriptionText = value;
                OnPropertyChanged(nameof(DescriptionText));
            }
        }
        [DataSourceProperty]
        public SettingType SettingType => SettingProperty.SettingType;
        [DataSourceProperty]
        public float MinValue => SettingProperty.SettingPropertyDefinition.EditableMinValue;
        [DataSourceProperty]
        public float MaxValue => SettingProperty.SettingPropertyDefinition.EditableMaxValue;

        public EditValueVM(SettingPropertyVM settingProperty)
        {
            SettingProperty = settingProperty;

            TitleText = $"Edit \"{SettingProperty.Name}\"";
            switch (SettingType)
            {
                case SettingType.Bool:
                case SettingType.Dropdown:
                    // Won't appear here
                    break;
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

            OnPropertyChanged(nameof(SettingType));
        }

        public void ExecuteDone()
        {
            if (SettingProperty.SettingType == SettingType.Int && int.TryParse(TextInput, out var intVal))
            {
                SettingProperty.URS.Do(new SetValueTypeAction<int>(new ProxyRef(() => SettingProperty.IntValue, o => SettingProperty.IntValue = (int) o), intVal));
            }
            else if (SettingProperty.SettingType == SettingType.Float && float.TryParse(TextInput, out var floatVal))
            {
                SettingProperty.URS.Do(new SetValueTypeAction<float>(new ProxyRef(() => SettingProperty.FloatValue, o => SettingProperty.FloatValue = (float) o), floatVal));
            }
            else if (SettingProperty.SettingType == SettingType.String)
            {
                SettingProperty.URS.Do(new SetStringAction(new ProxyRef(() => SettingProperty.StringValue, o => SettingProperty.StringValue = (string) o), TextInput));
            }
            ScreenManager.PopScreen();
        }

        public void ExecuteCancel()
        {
            ScreenManager.PopScreen();
        }
    }
}
