using System;
using HarmonyLib;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithCustomFormatterWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }

        public PropertyDefinitionWithCustomFormatterWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            CustomFormatter = AccessTools.Property(type, nameof(CustomFormatter))?.GetValue(@object) as Type;
        }

    }
}