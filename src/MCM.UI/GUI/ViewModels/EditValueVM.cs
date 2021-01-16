using System.Collections.Generic;
using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings;
using MCM.UI.Actions;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class EditValueVM : ViewModel
    {
        private string _textInput = string.Empty;
        private string _titleText = "title";
        private string _descriptionText = "description";
        private string _cancelButtonText = string.Empty;
        private string _doneButtonText = string.Empty;

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
        [DataSourceProperty]
        public string DoneButtonText
        {
            get => _doneButtonText;
            set
            {
                _doneButtonText = value;
                OnPropertyChanged(nameof(DoneButtonText));
            }
        }
        [DataSourceProperty]
        public string CancelButtonText
        {
            get => _cancelButtonText;
            set
            {
                _cancelButtonText = value;
                OnPropertyChanged(nameof(CancelButtonText));
            }
        }

        public EditValueVM(SettingsPropertyVM settingProperty)
        {
            SettingProperty = settingProperty;

            TitleText = TextObjectHelper.Create("{=EditValueVM_TitleText}Edit \"{PROPERTYNAME}\"", new Dictionary<string, TextObject>()
            {
                { "PROPERTYNAME", TextObjectHelper.Create(SettingProperty.Name) }
            }).ToString();
            switch (SettingType)
            {
                case SettingType.Int:
                case SettingType.Float:
                {
                    var format = SettingProperty.IsIntVisible ? "0" : "0.00";
                    DescriptionText = TextObjectHelper.Create("{=EditValueVM_DescriptionText_Numeric}Edit the value for \"{PROPERTYNAME}\".\nThe minimum value is {MINVALUE} and the maximum value is {MAXVALUE}.", new Dictionary<string, TextObject>()
                    {
                        { "PROPERTYNAME", TextObjectHelper.Create(SettingProperty.Name) },
                        { "MINVALUE", TextObjectHelper.Create(SettingProperty.SettingPropertyDefinition.EditableMinValue.ToString(format)) },
                        { "MAXVALUE", TextObjectHelper.Create(SettingProperty.SettingPropertyDefinition.EditableMaxValue.ToString(format)) },
                    }).ToString();
                    break;
                }
                case SettingType.String:
                {
                    DescriptionText = TextObjectHelper.Create("{=EditValueVM_DescriptionText_Text}Edit the value for \"{PROPERTYNAME}\".", new Dictionary<string, TextObject>()
                    {
                        { "PROPERTYNAME", TextObjectHelper.Create(SettingProperty.Name) },
                    }).ToString();
                    break;
                }
            }
            DoneButtonText = TextObjectHelper.Create("{=WiNRdfsm}Done").ToString();
            CancelButtonText = TextObjectHelper.Create("{=3CpNUnVl}Cancel").ToString();

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            TextInput = SettingProperty.SettingType switch
            {
                SettingType.Int => SettingProperty.IntValue.ToString(),
                SettingType.Float => SettingProperty.FloatValue.ToString(),
                SettingType.String => SettingProperty.StringValue,
                _ => string.Empty,
            };
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