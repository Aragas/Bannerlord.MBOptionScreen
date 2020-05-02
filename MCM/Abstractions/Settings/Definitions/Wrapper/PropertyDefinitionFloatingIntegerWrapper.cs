namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionFloatingIntegerWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionFloatingInteger
    {
        public float MinValue { get; }
        public float MaxValue { get; }
        public string ValueFormat { get; }

        internal PropertyDefinitionFloatingIntegerWrapper(object @object) : base(@object)
        {
            MinValue = @object.GetType().GetProperty(nameof(MinValue))?.GetValue(@object) as float? ?? 0;
            MaxValue = @object.GetType().GetProperty(nameof(MaxValue))?.GetValue(@object) as float? ?? 0;
            ValueFormat = @object.GetType().GetProperty(nameof(ValueFormat))?.GetValue(@object) as string ?? "0.00";
        }
    }
}