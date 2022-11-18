using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Common;
using MCM.UI.Actions;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private string _name = string.Empty;

        public ModOptionsVM MainView => SettingsVM.MainView;
        public SettingsVM SettingsVM { get; }
        public SettingsPropertyGroupVM? Group { get; set; }

        public UndoRedoStack URS => SettingsVM.URS;

        public ISettingsPropertyDefinition SettingPropertyDefinition { get; }
        public IRef PropertyReference => SettingPropertyDefinition.PropertyReference;
        public SettingType SettingType => SettingPropertyDefinition.SettingType;
        public string HintText => SettingPropertyDefinition.HintText.Length > 0 ? $"{Name}: {new TextObject(SettingPropertyDefinition.HintText)}" : string.Empty;
        public string ValueFormat => SettingPropertyDefinition.ValueFormat;
        public IFormatProvider? ValueFormatProvider { get; }

        public bool SatisfiesSearch => string.IsNullOrEmpty(MainView.SearchText) ||
                                       Name.IndexOf(MainView.SearchText, StringComparison.InvariantCultureIgnoreCase) >= 0;

        [DataSourceProperty]
        public string Name { get => _name; private set => SetField(ref _name, value, nameof(Name)); }
        [DataSourceProperty]
        public bool IsEnabled => Group?.GroupToggle ?? false;

        [DataSourceProperty]
        public bool IsSettingVisible
        {
            get
            {
                if (SettingPropertyDefinition.IsToggle)
                    return false;
                if (Group?.GroupToggle == false)
                    return false;
                if (Group?.IsExpanded == false)
                    return false;
                if (!SatisfiesSearch)
                    return false;
                return true;
            }
        }

        public bool IsSelected { get; set; }

        public SettingsPropertyVM(ISettingsPropertyDefinition definition, SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
            SettingPropertyDefinition = definition;
            ValueFormatProvider = SettingPropertyDefinition.CustomFormatter is not null
                ? Activator.CreateInstance(SettingPropertyDefinition.CustomFormatter) as IFormatProvider
                : null;

            NumericValueToggle = IsInt || IsFloat;

            PropertyReference.PropertyChanged += OnPropertyChanged;

            if (IsDropdown)
            {
                if (PropertyReference.Value is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
                DropdownValue.PropertyChanged += DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue += DropdownValue_PropertyChangedWithValue;
            }

            // Can easily backfire as I do not hold the reference
            if (SettingsVM.SettingsInstance is INotifyPropertyChanged notifyPropertyChanged2)
                notifyPropertyChanged2.PropertyChanged += OnPropertyChanged;

            RefreshValues();

            if (MCMUISubModule.ResetValueToDefault is { } key)
                key.IsDownAndReleasedEvent += ResetValueToDefaultOnReleasedEvent;
        }
        public override void OnFinalize()
        {
            if (MCMUISubModule.ResetValueToDefault is { } key)
                key.IsDownAndReleasedEvent -= ResetValueToDefaultOnReleasedEvent;

            PropertyReference.PropertyChanged -= OnPropertyChanged;

            if (IsDropdown)
            {
                if (PropertyReference.Value is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged -= OnPropertyChanged;
                DropdownValue.PropertyChanged -= DropdownValue_PropertyChanged;
                DropdownValue.PropertyChangedWithValue -= DropdownValue_PropertyChangedWithValue;
            }

            // Can easily backfire as I do not hold the reference
            if (SettingsVM.SettingsInstance is INotifyPropertyChanged notifyPropertyChanged2)
                notifyPropertyChanged2.PropertyChanged -= OnPropertyChanged;

            base.OnFinalize();
        }
        private void OnPropertyChanged(object? obj, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == BaseSettings.SaveTriggered)
                return;

            switch (SettingType)
            {
                case SettingType.Bool:
                    OnPropertyChanged(nameof(BoolValue));
                    break;
                case SettingType.Int:
                    OnPropertyChanged(nameof(IntValue));
                    OnPropertyChanged(nameof(NumericValue));
                    break;
                case SettingType.Float:
                    OnPropertyChanged(nameof(FloatValue));
                    OnPropertyChanged(nameof(NumericValue));
                    break;
                case SettingType.String:
                    OnPropertyChanged(nameof(StringValue));
                    break;
                case SettingType.Dropdown:
                    DropdownValue.SelectedIndex = new SelectedIndexWrapper(PropertyReference.Value).SelectedIndex;
                    break;
                case SettingType.Button:
                    ButtonContent = new TextObject(SettingPropertyDefinition.Content).ToString();
                    break;
            }
            SettingsVM.RecalculateIndex();
        }

        private void ResetValueToDefaultOnReleasedEvent()
        {
            if (IsSelected)
            {
                SettingsVM.ResetSettingsValue(SettingPropertyDefinition.Id);

                switch (SettingType)
                {
                    case SettingType.Bool:
                        OnPropertyChanged(nameof(BoolValue));
                        break;
                    case SettingType.Int:
                        OnPropertyChanged(nameof(IntValue));
                        OnPropertyChanged(nameof(NumericValue));
                        break;
                    case SettingType.Float:
                        OnPropertyChanged(nameof(FloatValue));
                        OnPropertyChanged(nameof(NumericValue));
                        break;
                    case SettingType.String:
                        OnPropertyChanged(nameof(StringValue));
                        break;
                    case SettingType.Dropdown:
                        DropdownValue.SelectedIndex = new SelectedIndexWrapper(PropertyReference.Value).SelectedIndex;
                        break;
                    case SettingType.Button:
                        break;
                }
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            Name = new TextObject(SettingPropertyDefinition.DisplayName).ToString();
            ButtonContent = new TextObject(SettingPropertyDefinition.Content).ToString();
            DropdownValue.RefreshValues();
        }


        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Reflection is used.")]
        public void OnHover()
        {
            IsSelected = true;
            MainView.HintText = HintText;
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Reflection is used.")]
        public void OnHoverEnd()
        {
            IsSelected = false;
            MainView.HintText = string.Empty;
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Reflection is used.")]
        public void OnValueClick()
        {
            if (PropertyReference.Value is Action val)
            {
                val();
            }
        }

        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}