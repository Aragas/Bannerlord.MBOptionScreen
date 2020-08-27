extern alias v13;

using MCM.Abstractions.Settings.Definitions;

using LegacyAttribute = v13::ModLib.Definitions.Attributes.SettingPropertyGroupAttribute;

namespace MCM.Implementation.ModLib.Attributes.v13
{
    public sealed class ModLibDefinitionsPropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public ModLibDefinitionsPropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(LegacyAttribute.GroupName))?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty(nameof(LegacyAttribute.IsMainToggle))?.GetValue(@object) as bool? ?? false;
            GroupOrder = -1;
        }
    }
}