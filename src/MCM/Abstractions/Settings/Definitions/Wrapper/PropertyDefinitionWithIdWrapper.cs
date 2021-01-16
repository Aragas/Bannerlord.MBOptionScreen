namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithIdWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithId
    {
        public string Id { get; }

        public PropertyDefinitionWithIdWrapper(object @object) : base(@object) { }
    }
}