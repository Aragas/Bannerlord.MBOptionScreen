namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionIntegerWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionInteger
    {
        public int MinValue { get; }
        public int MaxValue { get; }
        public string ValueFormat { get; }

        internal PropertyDefinitionIntegerWrapper(object @object) : base(@object)
        {
            MinValue = @object.GetType().GetProperty(nameof(MinValue))?.GetValue(@object) as int? ?? 0;
            MaxValue = @object.GetType().GetProperty(nameof(MaxValue))?.GetValue(@object) as int? ?? 0;
            ValueFormat = @object.GetType().GetProperty(nameof(ValueFormat))?.GetValue(@object) as string ?? "0";
        }
    }
}