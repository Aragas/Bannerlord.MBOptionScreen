namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithFormatWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithFormat
    {
        public string ValueFormat { get; }

        public PropertyDefinitionWithFormatWrapper(object @object) : base(@object)
        {
            ValueFormat = @object.GetType().GetProperty(nameof(ValueFormat))?.GetValue(@object) as string ?? "";
        }
    }
}