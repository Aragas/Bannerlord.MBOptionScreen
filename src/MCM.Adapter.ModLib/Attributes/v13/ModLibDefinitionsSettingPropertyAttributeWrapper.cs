extern alias v13;

using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Definitions;

using System;

using v13SettingPropertyAttribute = v13::ModLib.Definitions.Attributes.SettingPropertyAttribute;

namespace MCM.Adapter.ModLib.Attributes.v13
{
    internal sealed class ModLibDefinitionsSettingPropertyAttributeWrapper :
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

            DisplayName = TextObjectHelper.Create(type.GetProperty(nameof(v13SettingPropertyAttribute.DisplayName))?.GetValue(@object) as string ?? "ERROR")?.ToString() ?? "ERROR";
            HintText = TextObjectHelper.Create(type.GetProperty(nameof(v13SettingPropertyAttribute.HintText))?.GetValue(@object) as string ?? "ERROR")?.ToString() ?? "ERROR";
            Order = -1;
            RequireRestart = true;

            MinValue = (decimal) (type.GetProperty(nameof(v13SettingPropertyAttribute.MinValue))?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (type.GetProperty(nameof(v13SettingPropertyAttribute.MaxValue))?.GetValue(@object) as float? ?? 0);
            EditableMinValue = (decimal) (type.GetProperty(nameof(v13SettingPropertyAttribute.EditableMinValue))?.GetValue(@object) as float? ?? 0);
            EditableMaxValue = (decimal) (type.GetProperty(nameof(v13SettingPropertyAttribute.EditableMaxValue))?.GetValue(@object) as float? ?? 0);
        }
    }
}