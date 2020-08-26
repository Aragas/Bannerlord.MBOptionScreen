extern alias v4;

using HarmonyLib;

using System;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    public sealed class MCMv3PropertyDefinitionWithActionFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithActionFormat
    {
        /// <inheritdoc/>
        public Func<object, string> ValueFormatFunc { get; }

        public MCMv3PropertyDefinitionWithActionFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormatFunc = AccessTools.Property(type, nameof(ValueFormatFunc))?.GetValue(@object) as Func<object, string> ?? (obj => obj.ToString());
        }
    }
}