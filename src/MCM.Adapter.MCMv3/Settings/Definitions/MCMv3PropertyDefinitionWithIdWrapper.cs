extern alias v4;

using HarmonyLib;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithIdWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithId
    {
        /// <inheritdoc/>
        public string Id { get; }

        public MCMv3PropertyDefinitionWithIdWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            Id = AccessTools.Property(type, nameof(Id))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}