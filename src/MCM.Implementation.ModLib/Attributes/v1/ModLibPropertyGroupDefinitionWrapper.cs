extern alias v1;

using MCM.Abstractions.Settings.Definitions;

using v1SettingPropertyAttribute = v1::ModLib.Attributes.SettingPropertyGroupAttribute;

namespace MCM.Implementation.ModLib.Attributes.v1
{
    internal sealed class ModLibPropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public ModLibPropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(v1SettingPropertyAttribute.GroupName))?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty(nameof(v1SettingPropertyAttribute.IsMainToggle))?.GetValue(@object) as bool? ?? false;
            GroupOrder = -1;
        }
    }
}