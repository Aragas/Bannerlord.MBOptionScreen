using HarmonyLib;

using System;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithActionFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithActionFormat
    {
        public Func<object, string> ValueFormatFunc { get; }

        public PropertyDefinitionWithActionFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormatFunc = AccessTools.Property(type, nameof(ValueFormatFunc))?.GetValue(@object) as Func<object, string> ?? (obj => obj.ToString());
        }
    }
}