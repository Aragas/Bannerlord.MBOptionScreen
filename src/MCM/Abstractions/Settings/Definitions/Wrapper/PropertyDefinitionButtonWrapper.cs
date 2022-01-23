using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionButtonWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionButton
    {
        public string Content { get; }

        public PropertyDefinitionButtonWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            Content = AccessTools2.Property(type, nameof(Content))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}