using MBOptionScreen.Settings;

namespace MBOptionScreen.Attributes
{
    /// <summary>
    /// Wrapper for SettingPropertyGroupAttribute. I think it world be better to make a model for it.
    /// </summary>
    internal sealed class PropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int Order { get; }

        public PropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty("GroupName")?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty("IsMainToggle")?.GetValue(@object) as bool? ?? false;
            Order = type.GetProperty("Order")?.GetValue(@object) as int? ?? -1;
        }
    }
}