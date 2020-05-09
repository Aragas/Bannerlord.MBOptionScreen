using MCM.Abstractions.Settings.Definitions;

using TaleWorlds.Localization;

namespace MCM.Implementation.MBO.Attributes
{
    public sealed class MBOSettingPropertyAttributeWrapper : 
        IPropertyDefinitionBool,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionText,
        IPropertyDefinitionDropdown
    {
        public string DisplayName { get; }
        public int Order { get; }
        public bool RequireRestart { get; }
        public string HintText { get; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; }
        public int SelectedIndex { get; }
        public float EditableMinValue { get; }
        public float EditableMaxValue { get; }

        public MBOSettingPropertyAttributeWrapper(object @object)
        {
            var type = @object.GetType();

            DisplayName = new TextObject(type.GetProperty("Name")?.GetValue(@object) as string ?? "ERROR", null).ToString();
            HintText = new TextObject(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR", null).ToString();
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? 0;
            RequireRestart = type.GetProperty(nameof(RequireRestart))?.GetValue(@object) as bool? ?? true;

            MinValue = (decimal) (type.GetProperty(nameof(MinValue))?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty(nameof(MaxValue))?.GetValue(@object) as float? ?? 0);
            ValueFormat = "";
            SelectedIndex = type.GetProperty(nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
            EditableMinValue = type.GetProperty(nameof(EditableMinValue))?.GetValue(@object) as float? ?? 0;
            EditableMaxValue = type.GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as float? ?? 0;
        }
    }
}