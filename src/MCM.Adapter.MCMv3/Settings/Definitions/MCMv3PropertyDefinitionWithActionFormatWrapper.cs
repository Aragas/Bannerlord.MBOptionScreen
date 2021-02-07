extern alias v4;

using System;

using HarmonyLib;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithActionFormatWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithActionFormat
    {
        /// <inheritdoc/>
        public Func<object, string> ValueFormatFunc { get; }

        public MCMv3PropertyDefinitionWithActionFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormatFunc = AccessTools.Property(type, nameof(ValueFormatFunc))?.GetValue(@object) as Func<object, string> ?? (obj => obj.ToString() ?? string.Empty);
        }
    }
}
