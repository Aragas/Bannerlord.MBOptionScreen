extern alias v4;

using v4::MCM.Abstractions.Settings.Definitions;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyGroupDefinitionWrapper :
        IPropertyGroupDefinition
    {
        /// <inheritdoc/>
        public string GroupName { get; }

        /// <inheritdoc/>
        public bool IsMainToggle { get; }

        /// <inheritdoc/>
        public int GroupOrder { get; }

        public MCMv3PropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty("GroupName")?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty("IsMainToggle")?.GetValue(@object) as bool? ?? false;
            GroupOrder = type.GetProperty("Order")?.GetValue(@object) as int? ?? -1;
        }
    }
}