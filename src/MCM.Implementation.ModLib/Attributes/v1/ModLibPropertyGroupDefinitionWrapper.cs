extern alias v1;

using MCM.Abstractions.Settings.Definitions;

using LegacyAttribute = v1::ModLib.Attributes.SettingPropertyGroupAttribute;

namespace MCM.Implementation.ModLib.Attributes.v1
{
    public sealed class ModLibPropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public ModLibPropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(LegacyAttribute.GroupName))?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty(nameof(LegacyAttribute.IsMainToggle))?.GetValue(@object) as bool? ?? false;
            GroupOrder = -1;
        }
    }
}