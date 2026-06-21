using Bannerlord.ButterLib.Common.Helpers;

using MCM.Abstractions;
using MCM.Common;
using MCM.UI.Actions;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private static TextObject SetNumeric(TextObject textObject, int value)
        {
            LocalizationHelper.SetNumericVariable(textObject, "NUMERIC", value);
            return textObject;
        }
        private static TextObject SetNumeric(TextObject textObject, float value)
        {
            LocalizationHelper.SetNumericVariable(textObject, "NUMERIC", value);
            return textObject;
        }

        [DataSourceProperty]
        public bool IsIntVisible { get; private set; }
        [DataSourceProperty]
        public bool IsFloatVisible { get; private set; }

        [DataSourceProperty]
        public bool IsInt => SettingType == SettingType.Int;
        [DataSourceProperty]
        public bool IsFloat => SettingType == SettingType.Float;

        [DataSourceProperty]
        public float FloatValue
        {
            get => IsFloat ? PropertyReference.Value is float val ? val : float.MinValue : 0f;
            set
            {
                value = MathF.Max(MathF.Min(value, MaxFloat), MinFloat);
                if (IsFloat && MathF.Abs(FloatValue - value) >= Constants.Tolerance) // Todo: check other float comparisons
                {
                    URS.Do(new SetValueTypeAction<float>(PropertyReference, value));
                    OnPropertyChanged(nameof(FloatValue));
                    OnPropertyChanged(nameof(NumericValue));
                }
            }
        }
        [DataSourceProperty]
        public int IntValue
        {
            get => IsInt ? PropertyReference.Value is int val ? val : int.MinValue : 0;
            set
            {
                value = MathF.Max(MathF.Min(value, MaxInt), MinInt);
                if (IsInt && IntValue != value)
                {
                    URS.Do(new SetValueTypeAction<int>(PropertyReference, value));
                    OnPropertyChanged(nameof(IntValue));
                    OnPropertyChanged(nameof(NumericValue));
                }
            }
        }

        [DataSourceProperty]
        public int MaxInt => (int) SettingPropertyDefinition.MaxValue;
        [DataSourceProperty]
        public int MinInt => (int) SettingPropertyDefinition.MinValue;

        [DataSourceProperty]
        public float MaxFloat => (float) SettingPropertyDefinition.MaxValue;
        [DataSourceProperty]
        public float MinFloat => (float) SettingPropertyDefinition.MinValue;

        [DataSourceProperty]
        public bool IsNotNumeric => !(IsInt || IsFloat);

        [DataSourceProperty]
        public bool NumericValueToggle { get; private set; }

        [DataSourceProperty]
        public string NumericValue => SettingType switch
        {
            SettingType.Int when PropertyReference.Value is int val => string.IsNullOrWhiteSpace(ValueFormat)
                ? string.Format(ValueFormatProvider, "{0}", val.ToString("0"))
                : string.Format(ValueFormatProvider, "{0}", val.ToString(SetNumeric(new TextObject(ValueFormat), val).ToString())),
            SettingType.Float when PropertyReference.Value is float val => string.IsNullOrWhiteSpace(ValueFormat)
                ? string.Format(ValueFormatProvider, "{0}", val.ToString("0.00"))
                : string.Format(ValueFormatProvider, "{0}", val.ToString(SetNumeric(new TextObject(ValueFormat), val).ToString())),
            _ => string.Empty
        };

        public void OnEditBoxHover()
        {
            switch (SettingType)
            {
                case SettingType.Float:
                    IsFloatVisible = !IsFloatVisible;
                    NumericValueToggle = !NumericValueToggle;
                    OnPropertyChanged(nameof(IsFloatVisible));
                    OnPropertyChanged(nameof(NumericValueToggle));
                    break;
                case SettingType.Int:
                    IsIntVisible = !IsIntVisible;
                    NumericValueToggle = !NumericValueToggle;
                    OnPropertyChanged(nameof(IsIntVisible));
                    OnPropertyChanged(nameof(NumericValueToggle));
                    break;
            }
        }

        public void OnEditBoxHoverEnd()
        {
            switch (SettingType)
            {
                case SettingType.Float:
                    IsFloatVisible = !IsFloatVisible;
                    NumericValueToggle = !NumericValueToggle;
                    OnPropertyChanged(nameof(IsFloatVisible));
                    OnPropertyChanged(nameof(NumericValueToggle));
                    break;
                case SettingType.Int:
                    IsIntVisible = !IsIntVisible;
                    NumericValueToggle = !NumericValueToggle;
                    OnPropertyChanged(nameof(IsIntVisible));
                    OnPropertyChanged(nameof(NumericValueToggle));
                    break;
            }
        }
    }
}