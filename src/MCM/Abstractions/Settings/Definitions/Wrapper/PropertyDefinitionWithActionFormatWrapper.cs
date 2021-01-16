using System;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithActionFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithActionFormat
    {
        public Func<object, string> ValueFormatFunc { get; }

        public PropertyDefinitionWithActionFormatWrapper(object @object) : base(@object) { }
    }
}