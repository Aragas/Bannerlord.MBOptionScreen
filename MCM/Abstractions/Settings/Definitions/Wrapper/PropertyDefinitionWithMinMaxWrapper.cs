namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithMinMax
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }

        public PropertyDefinitionWithMinMaxWrapper(object @object) : base(@object)
        {
            MinValue = @object.GetType().GetProperty(nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = @object.GetType().GetProperty(nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}