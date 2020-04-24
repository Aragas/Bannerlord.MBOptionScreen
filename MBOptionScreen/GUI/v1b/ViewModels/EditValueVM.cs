using MBOptionScreen.Actions;
using MBOptionScreen.Settings;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1b.ViewModels
{
    public class EditValueVM : ViewModel
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
                    OnPropertyChanged();

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
                OnPropertyChanged();
            }
        }
        [DataSourceProperty]
        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                _descriptionText = value;
                OnPropertyChanged();
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

            if(SettingProperty.SettingType == SettingType.Int)
                TextInput = ((int)SettingProperty.Property.GetValue(SettingProperty.SettingsInstance)).ToString("0") ?? "";
            else
            if (SettingProperty.SettingType == SettingType.Float)
                TextInput = ((float)SettingProperty.Property.GetValue(SettingProperty.SettingsInstance)).ToString("0.00") ?? "";
            else
                TextInput = SettingProperty.Property.GetValue(SettingProperty.SettingsInstance).ToString() ?? "";

            OnPropertyChanged(nameof(SettingType));
        }

        public void ExecuteDone()
        {
            if (SettingProperty.SettingType == SettingType.Int)
            {
                if (int.TryParse(TextInput, out var val))
                {
                    SettingProperty.URS.Do(new SetIntSettingProperty(SettingProperty, val));
                    SettingProperty.URS.Do(new SetValueAction<int>(new Ref(SettingProperty.Property, SettingProperty.SettingsInstance), val));
                    SettingProperty.OnPropertyChanged(nameof(SettingProperty.ValueString));
                }
            }
            else if (SettingProperty.SettingType == SettingType.Float)
            {
                if (float.TryParse(TextInput, out var val))
                {
                    SettingProperty.URS.Do(new SetFloatSettingProperty(SettingProperty, val));
                    SettingProperty.URS.Do(new SetValueAction<float>(new Ref(SettingProperty.Property, SettingProperty.SettingsInstance), val));
                    SettingProperty.OnPropertyChanged(nameof(SettingProperty.ValueString));
                }
            }
            else if (SettingProperty.SettingType == SettingType.String)
            {
                SettingProperty.URS.Do(new SetStringSettingProperty(SettingProperty, TextInput));
                SettingProperty.URS.Do(new SetValueAction<string>(new Ref(SettingProperty.Property, SettingProperty.SettingsInstance), TextInput));
                SettingProperty.OnPropertyChanged(nameof(SettingProperty.ValueString));
            }
            ScreenManager.PopScreen();
        }

        public void ExecuteCancel()
        {
            ScreenManager.PopScreen();
        }
    }
}
