using HarmonyLib.BUTR.Extensions;

using System;

namespace MCM.Abstractions.Wrapper
{
    public sealed class PropertyDefinitionWithActionFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithActionFormat
    {
        /// <inheritdoc/>
        public Func<object, string> ValueFormatFunc { get; }

        public PropertyDefinitionWithActionFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormatFunc = AccessTools2.Property(type, nameof(ValueFormatFunc))?.GetValue(@object) as Func<object, string> ?? (obj => obj.ToString());
        }
    }
}