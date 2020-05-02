namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    /// <summary>
    /// Wrapper for SettingPropertyGroupAttribute. I think it world be better to make a model for it.
    /// </summary>
    public sealed class PropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int Order { get; }

        public PropertyGroupDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            GroupName = type.GetProperty(nameof(GroupName))?.GetValue(@object) as string ?? "ERROR";
            IsMainToggle = type.GetProperty(nameof(IsMainToggle))?.GetValue(@object) as bool? ?? false;
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? -1;
        }
    }
}