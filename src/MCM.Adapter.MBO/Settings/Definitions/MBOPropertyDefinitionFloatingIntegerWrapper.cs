extern alias v4;

using HarmonyLib.BUTR.Extensions;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MBO.Settings.Definitions
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

            MinValue = (decimal) (AccessTools2.Property(type, "MinValue")?.GetValue(@object) as float? ?? 0);
            MaxValue = (decimal) (AccessTools2.Property(type, "MaxValue")?.GetValue(@object) as float? ?? 0);
            ValueFormat = AccessTools2.Property(type, "ValueFormat")?.GetValue(@object) as string ?? "0.00";
        }
    }
}