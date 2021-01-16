using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Definitions;

namespace MCM.Abstractions.Attributes.v1
{
    public sealed class SettingPropertyAttributeWrapper : IWrapper,
        IPropertyDefinitionBool,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionText,
        IPropertyDefinitionDropdown
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public string DisplayName { get; }
        public int Order { get; }
        public bool RequireRestart { get; }
        public string HintText { get; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; }
        public int SelectedIndex { get; }
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }

        public SettingPropertyAttributeWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            DisplayName = TextObjectHelper.Create(type.GetProperty("Name")?.GetValue(@object) as string ?? "ERROR").ToString();
            HintText = TextObjectHelper.Create(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR").ToString();
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? 0;
            RequireRestart = type.GetProperty(nameof(RequireRestart))?.GetValue(@object) as bool? ?? true;

            MinValue = type.GetProperty(nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = type.GetProperty(nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
            ValueFormat = "";
            SelectedIndex = type.GetProperty(nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
            EditableMinValue = type.GetProperty(nameof(EditableMinValue))?.GetValue(@object) as decimal? ?? 0;
            EditableMaxValue = type.GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as decimal? ?? 0;

            IsCorrect = true;
        }
    }
}