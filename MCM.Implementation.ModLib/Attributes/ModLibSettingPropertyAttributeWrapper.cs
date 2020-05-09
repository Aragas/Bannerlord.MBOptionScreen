using MCM.Abstractions.Settings.Definitions;

using TaleWorlds.Localization;

namespace MCM.Implementation.ModLib.Attributes
{
    public sealed class ModLibSettingPropertyAttributeWrapper :
        IPropertyDefinitionBool,
        IPropertyDefinitionWithMinMax
    {
        public string DisplayName { get; }
        public int Order { get; }
        public bool RequireRestart { get; }
        public string HintText { get; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public float EditableMinValue { get; }
        public float EditableMaxValue { get; }

        public ModLibSettingPropertyAttributeWrapper(object @object)
        {
            var type = @object.GetType();

            DisplayName = new TextObject((type.GetProperty(nameof(DisplayName)) ?? type.GetProperty("Name"))?.GetValue(@object) as string ?? "ERROR", null).ToString();
            HintText = new TextObject(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR", null).ToString();
            Order = -1;
            RequireRestart = true;

            MinValue = (decimal) (type.GetProperty(nameof(MinValue))?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty(nameof(MaxValue))?.GetValue(@object) as float? ?? 0);
            EditableMinValue = type.GetProperty(nameof(EditableMinValue))?.GetValue(@object) as float? ?? 0;
            EditableMaxValue = type.GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as float? ?? 0;
        }
    }
}