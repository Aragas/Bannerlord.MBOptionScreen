using MCM.Abstractions.Settings.Definitions;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Attributes.v1
{
    public sealed class SettingPropertyAttributeWrapper : IWrapper,
        IPropertyDefinitionBool,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionText,
        IPropertyDefinitionDropdown
    {
        /// <inheritdoc/>
        public object Object { get; }
        /// <inheritdoc/>
        public bool IsCorrect { get; }

        /// <inheritdoc/>
        public string DisplayName { get; }
        /// <inheritdoc/>
        public int Order { get; }
        /// <inheritdoc/>
        public bool RequireRestart { get; }
        /// <inheritdoc/>
        public string HintText { get; }
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }
        /// <inheritdoc/>
        public string ValueFormat { get; }
        /// <inheritdoc/>
        public int SelectedIndex { get; }
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }

        public SettingPropertyAttributeWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            DisplayName = new TextObject(type.GetProperty("Name")?.GetValue(@object) as string ?? "ERROR").ToString();
            HintText = new TextObject(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR").ToString();
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