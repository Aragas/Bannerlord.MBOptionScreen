extern alias v4;

using HarmonyLib;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

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
            var type = @object.GetType();

            MinValue = (decimal) (AccessTools.Property(type, "MinValue")?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (AccessTools.Property(type, "MaxValue")?.GetValue(@object) as float? ?? 0);
            ValueFormat = AccessTools.Property(type, "ValueFormat")?.GetValue(@object) as string ?? "0.00";
        }
    }
}