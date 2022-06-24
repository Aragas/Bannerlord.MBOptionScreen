using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithFormatWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithFormat
    {
        /// <inheritdoc/>
        public string ValueFormat { get; }

        public PropertyDefinitionWithFormatWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            ValueFormat = AccessTools2.Property(type, nameof(ValueFormat))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}