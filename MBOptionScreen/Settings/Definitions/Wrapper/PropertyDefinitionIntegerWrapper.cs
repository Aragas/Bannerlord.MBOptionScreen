namespace MBOptionScreen.Settings
{
    internal sealed class PropertyDefinitionIntegerWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionInteger
    {
        public int MinValue { get; }
        public int MaxValue { get; }
        public string ValueFormat { get; }

        internal PropertyDefinitionIntegerWrapper(object @object) : base(@object)
        {
            MinValue = @object.GetType().GetProperty("MinValue")?.GetValue(@object) as int? ?? 0;
            MaxValue = @object.GetType().GetProperty("MaxValue")?.GetValue(@object) as int? ?? 0;
            ValueFormat = @object.GetType().GetProperty("ValueFormat")?.GetValue(@object) as string ?? "0";
        }
    }
}