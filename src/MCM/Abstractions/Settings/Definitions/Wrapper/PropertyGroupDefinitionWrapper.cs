using System;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        [Obsolete("Will be removed", true)]
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public PropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(GroupName))?.GetValue(@object) as string ?? "ERROR";
            GroupOrder = type.GetProperty(nameof(GroupOrder))?.GetValue(@object) as int? ?? -1;
        }
    }
}