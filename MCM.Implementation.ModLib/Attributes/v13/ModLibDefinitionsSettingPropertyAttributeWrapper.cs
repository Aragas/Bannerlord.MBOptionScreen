using MCM.Abstractions.Settings.Definitions;

using TaleWorlds.Localization;

namespace MCM.Implementation.ModLib.Attributes.v13
{
    public sealed class ModLibDefinitionsSettingPropertyAttributeWrapper :
        IPropertyDefinitionBool,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithEditableMinMax
    {
        public string DisplayName { get; }
        public int Order { get; }
        public bool RequireRestart { get; }
        public string HintText { get; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }

        public ModLibDefinitionsSettingPropertyAttributeWrapper(object @object)
        {
            var type = @object.GetType();

            DisplayName = new TextObject((type.GetProperty("DisplayName") ?? type.GetProperty("Name"))?.GetValue(@object) as string ?? "ERROR").ToString();
            HintText = new TextObject(type.GetProperty("HintText")?.GetValue(@object) as string ?? "ERROR").ToString();
            Order = -1;
            RequireRestart = true;

            MinValue = (decimal) (type.GetProperty("MinValue")?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty("MaxValue")?.GetValue(@object) as float? ?? 0);
            EditableMinValue = (decimal) (type.GetProperty("EditableMinValue")?.GetValue(@object) as float? ?? 0);
            EditableMaxValue = (decimal) (type.GetProperty("EditableMaxValue")?.GetValue(@object) as float? ?? 0);
        }
    }
}