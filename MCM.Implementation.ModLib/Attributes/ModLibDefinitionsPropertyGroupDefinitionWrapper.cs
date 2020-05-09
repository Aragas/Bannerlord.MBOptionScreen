using MCM.Abstractions.Settings.Definitions;

namespace MCM.Implementation.ModLib.Attributes
{
    public sealed class ModLibDefinitionsPropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public ModLibDefinitionsPropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty("GroupName")?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty("IsMainToggle")?.GetValue(@object) as bool? ?? false;
            GroupOrder = -1;
        }
    }
}