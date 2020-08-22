extern alias v4;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    public sealed class MCMv3PropertyDefinitionFloatingIntegerWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; }

        public MCMv3PropertyDefinitionFloatingIntegerWrapper(object @object) : base(@object)
        {
            MinValue = (decimal) (@object.GetType().GetProperty("MinValue")?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (@object.GetType().GetProperty("MaxValue")?.GetValue(@object) as float? ?? 0);
            ValueFormat = @object.GetType().GetProperty("ValueFormat")?.GetValue(@object) as string ?? "0.00";
        }
    }
}