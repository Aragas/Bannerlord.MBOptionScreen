namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyGroupDefinitionWrapper : IPropertyGroupDefinition
    {
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }

        public PropertyGroupDefinitionWrapper(object @object) { }
    }
}