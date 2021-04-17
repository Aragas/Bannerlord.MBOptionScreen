extern alias v4;

using System;

using HarmonyLib.BUTR.Extensions;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithCustomFormatterWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }

        public MCMv3PropertyDefinitionWithCustomFormatterWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            CustomFormatter = AccessTools2.Property(type, nameof(CustomFormatter))?.GetValue(@object) as Type;
        }
    }
}