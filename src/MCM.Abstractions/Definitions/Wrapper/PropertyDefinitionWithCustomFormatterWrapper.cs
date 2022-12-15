using HarmonyLib.BUTR.Extensions;

using System;

namespace MCM.Abstractions.Wrapper
{
    public sealed class PropertyDefinitionWithCustomFormatterWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }

        public PropertyDefinitionWithCustomFormatterWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            CustomFormatter = AccessTools2.Property(type, nameof(CustomFormatter), false)?.GetValue(@object) as Type;
        }
    }
}