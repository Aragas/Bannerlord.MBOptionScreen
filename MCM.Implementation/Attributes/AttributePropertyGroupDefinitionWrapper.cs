using MCM.Abstractions.Settings.Definitions;

namespace MCM.Implementation.Attributes
{
    internal sealed class AttributePropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public AttributePropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(GroupName))?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty(nameof(IsMainToggle))?.GetValue(@object) as bool? ?? false;
            GroupOrder = type.GetProperty(nameof(GroupOrder))?.GetValue(@object) as int? ?? -1;
        }
    }
}