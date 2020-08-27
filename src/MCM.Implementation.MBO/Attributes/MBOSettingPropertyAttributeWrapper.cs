using MCM.Abstractions.Settings.Definitions;

using TaleWorlds.Localization;

namespace MCM.Implementation.MBO.Attributes
{
    public sealed class MBOSettingPropertyAttributeWrapper : 
        IPropertyDefinitionBool,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithEditableMinMax,
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
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }

        public MBOSettingPropertyAttributeWrapper(object @object)
        {
            var type = @object.GetType();

            DisplayName = new TextObject((type.GetProperty("DisplayName") ?? type.GetProperty("Name"))?.GetValue(@object) as string ?? "ERROR").ToString();
            HintText = new TextObject(type.GetProperty("HintText")?.GetValue(@object) as string ?? "ERROR").ToString();
            Order = type.GetProperty("Order")?.GetValue(@object) as int? ?? 0;
            RequireRestart = type.GetProperty("RequireRestart")?.GetValue(@object) as bool? ?? true;

            MinValue = (decimal) (type.GetProperty("MinValue")?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty("MaxValue")?.GetValue(@object) as float? ?? 0);
            ValueFormat = type.GetProperty("ValueFormat")?.GetValue(@object) as string ?? string.Empty;
            SelectedIndex = type.GetProperty("SelectedIndex")?.GetValue(@object) as int? ?? 0;
            EditableMinValue = (decimal) (type.GetProperty("EditableMinValue")?.GetValue(@object) as float? ?? (float) MinValue);
            EditableMaxValue = (decimal) (type.GetProperty("EditableMaxValue")?.GetValue(@object) as float? ?? (float) MaxValue);
        }
    }
}