using System;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithCustomFormatterWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithCustomFormatter
    {
        public Type? CustomFormatter { get; }

        public PropertyDefinitionWithCustomFormatterWrapper(object @object) : base(@object) { }
    }
}