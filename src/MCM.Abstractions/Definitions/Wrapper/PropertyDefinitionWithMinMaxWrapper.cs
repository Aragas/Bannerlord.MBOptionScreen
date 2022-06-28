using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Wrapper
{
    public sealed class PropertyDefinitionWithMinMaxWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithMinMax
    {
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }

        public PropertyDefinitionWithMinMaxWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            MinValue = AccessTools2.Property(type, nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = AccessTools2.Property(type, nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}