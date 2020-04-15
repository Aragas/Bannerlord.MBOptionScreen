using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace ModLib.GUI.ViewModels
{
    public class EditValueVM : ViewModel
    {
        private string _textInput = "";
        private string _titleText = "title";
        private string _descriptionText = "description";

        public SettingProperty SettingProperty { get; set; } = null;

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
        public float MinValue => SettingProperty.SettingAttribute.EditableMinValue;
        [DataSourceProperty]
        public float MaxValue => SettingProperty.SettingAttribute.EditableMaxValue;

        public EditValueVM(SettingProperty settingProperty)
        {
            SettingProperty = settingProperty;

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            TitleText = $"Edit \"{SettingProperty.Name}\"";
            DescriptionText = $"Edit the value for \"{SettingProperty.Name}\".\nThe minimum value is {SettingProperty.SettingAttribute.EditableMinValue} and the maximum value is {SettingProperty.SettingAttribute.EditableMaxValue}.";
            TextInput = SettingProperty.ValueString;
            OnPropertyChanged("SettingType");
        }

        private void ExecuteDone()
        {
            //TODO::
        }

        public void ExecuteCancel()
        {
            ScreenManager.PopScreen();
        }
    }
}
