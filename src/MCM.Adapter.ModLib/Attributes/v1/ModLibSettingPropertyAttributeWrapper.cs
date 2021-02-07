extern alias v1;

using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Definitions;

using v1SettingPropertyAttribute = v1::ModLib.Attributes.SettingPropertyAttribute;

namespace MCM.Adapter.ModLib.Attributes.v1
{
    internal sealed class ModLibSettingPropertyAttributeWrapper :
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

            DisplayName = TextObjectHelper.Create(type.GetProperty(nameof(v1SettingPropertyAttribute.DisplayName))?.GetValue(@object) as string ?? "ERROR").ToString();
            HintText = TextObjectHelper.Create(type.GetProperty(nameof(v1SettingPropertyAttribute.HintText))?.GetValue(@object) as string ?? "ERROR").ToString();
            Order = -1;
            RequireRestart = true;

            MinValue = (decimal) (type.GetProperty(nameof(v1SettingPropertyAttribute.MinValue))?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty(nameof(v1SettingPropertyAttribute.MaxValue))?.GetValue(@object) as float? ?? 0);
            EditableMinValue = (decimal) (type.GetProperty(nameof(v1SettingPropertyAttribute.EditableMinValue))?.GetValue(@object) as float? ?? 0);
            EditableMaxValue = (decimal) (type.GetProperty(nameof(v1SettingPropertyAttribute.EditableMaxValue))?.GetValue(@object) as float? ?? 0);
        }
    }
}