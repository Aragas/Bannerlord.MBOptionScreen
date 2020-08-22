extern alias v4;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    public sealed class MCMv3PropertyDefinitionIntegerWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; }

        public MCMv3PropertyDefinitionIntegerWrapper(object @object) : base(@object)
        {
            MinValue = (decimal) (@object.GetType().GetProperty("MinValue")?.GetValue(@object) as int? ?? 0);
            MaxValue = (decimal) (@object.GetType().GetProperty("MaxValue")?.GetValue(@object) as int? ?? 0);
            ValueFormat = @object.GetType().GetProperty("ValueFormat")?.GetValue(@object) as string ?? "0";
        }
    }
}