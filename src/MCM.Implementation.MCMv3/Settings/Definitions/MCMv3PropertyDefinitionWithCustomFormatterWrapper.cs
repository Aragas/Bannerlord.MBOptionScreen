extern alias v4;

using HarmonyLib;

using System;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithCustomFormatterWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }

        public MCMv3PropertyDefinitionWithCustomFormatterWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            CustomFormatter = AccessTools.Property(type, nameof(CustomFormatter))?.GetValue(@object) as Type;
        }
    }
}