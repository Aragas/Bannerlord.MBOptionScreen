extern alias v4;

using HarmonyLib;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithFormat
    {
        /// <inheritdoc/>
        public string ValueFormat { get; }

        public MCMv3PropertyDefinitionWithFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormat = AccessTools.Property(type, nameof(ValueFormat))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}