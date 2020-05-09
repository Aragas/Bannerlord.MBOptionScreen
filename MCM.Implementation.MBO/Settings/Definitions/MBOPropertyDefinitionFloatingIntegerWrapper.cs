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
            MinValue = (decimal) (@object.GetType().GetProperty(nameof(MinValue))?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (@object.GetType().GetProperty(nameof(MaxValue))?.GetValue(@object) as float? ?? 0);
            ValueFormat = @object.GetType().GetProperty(nameof(ValueFormat))?.GetValue(@object) as string ?? "0.00";
        }
    }
}