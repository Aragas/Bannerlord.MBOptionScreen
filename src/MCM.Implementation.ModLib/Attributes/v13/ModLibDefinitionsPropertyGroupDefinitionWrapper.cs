extern alias v13;

using MCM.Abstractions.Settings.Definitions;

using v13SettingPropertyAttribute = v13::ModLib.Definitions.Attributes.SettingPropertyGroupAttribute;

namespace MCM.Implementation.ModLib.Attributes.v13
{
    internal sealed class ModLibDefinitionsPropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public ModLibDefinitionsPropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(v13SettingPropertyAttribute.GroupName))?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty(nameof(v13SettingPropertyAttribute.IsMainToggle))?.GetValue(@object) as bool? ?? false;
            GroupOrder = -1;
        }
    }
}