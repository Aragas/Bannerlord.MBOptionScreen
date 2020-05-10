using MCM.Abstractions.Settings.Definitions;

namespace MCM.Implementation.MBO.Attributes
{
    public sealed class MBOPropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public MBOPropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty("GroupName")?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty("IsMainToggle")?.GetValue(@object) as bool? ?? false;
            GroupOrder = type.GetProperty("Order")?.GetValue(@object) as int? ?? -1;
        }
    }
}