using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MBO.Settings.Definitions
{
    public sealed class MBOPropertyDefinitionFloatingIntegerWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; }

        public MBOPropertyDefinitionFloatingIntegerWrapper(object @object) : base(@object)
        {
            MinValue = @object.GetType().GetProperty(nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = @object.GetType().GetProperty(nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
            ValueFormat = @object.GetType().GetProperty(nameof(ValueFormat))?.GetValue(@object) as string ?? "0.00";
        }
    }
}