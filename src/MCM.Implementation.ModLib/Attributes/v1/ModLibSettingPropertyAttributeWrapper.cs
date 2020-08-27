extern alias v1;

using MCM.Abstractions.Settings.Definitions;

using TaleWorlds.Localization;

using LegacyAttribute = v1::ModLib.Attributes.SettingPropertyAttribute;

namespace MCM.Implementation.ModLib.Attributes.v1
{
    public sealed class ModLibSettingPropertyAttributeWrapper :
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

        public ModLibSettingPropertyAttributeWrapper(object @object)
        {
            var type = @object.GetType();

            DisplayName = new TextObject(type.GetProperty(nameof(LegacyAttribute.DisplayName))?.GetValue(@object) as string ?? "ERROR").ToString();
            HintText = new TextObject(type.GetProperty(nameof(LegacyAttribute.HintText))?.GetValue(@object) as string ?? "ERROR").ToString();
            Order = -1;
            RequireRestart = true;

            MinValue = (decimal) (type.GetProperty(nameof(LegacyAttribute.MinValue))?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty(nameof(LegacyAttribute.MaxValue))?.GetValue(@object) as float? ?? 0);
            EditableMinValue = (decimal) (type.GetProperty(nameof(LegacyAttribute.EditableMinValue))?.GetValue(@object) as float? ?? 0);
            EditableMaxValue = (decimal) (type.GetProperty(nameof(LegacyAttribute.EditableMaxValue))?.GetValue(@object) as float? ?? 0);
        }
    }
}