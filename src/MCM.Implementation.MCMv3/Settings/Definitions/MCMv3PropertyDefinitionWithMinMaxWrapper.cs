extern alias v4;

using HarmonyLib;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithMinMaxWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithMinMax
    {
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }

        public MCMv3PropertyDefinitionWithMinMaxWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            MinValue = AccessTools.Property(type, nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = AccessTools.Property(type, nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}