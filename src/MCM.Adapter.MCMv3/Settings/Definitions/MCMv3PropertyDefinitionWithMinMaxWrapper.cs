extern alias v4;

using HarmonyLib.BUTR.Extensions;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
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

            MinValue = AccessTools2.Property(type, nameof(MinValue))?.GetValue(@object) as decimal? ?? 0;
            MaxValue = AccessTools2.Property(type, nameof(MaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}