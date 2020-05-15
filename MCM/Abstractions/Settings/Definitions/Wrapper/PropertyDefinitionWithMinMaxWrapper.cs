using HarmonyLib;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithMinMax
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }

        public PropertyDefinitionWithMinMaxWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            MinValue = AccessTools.Property(type, nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = AccessTools.Property(type, nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}