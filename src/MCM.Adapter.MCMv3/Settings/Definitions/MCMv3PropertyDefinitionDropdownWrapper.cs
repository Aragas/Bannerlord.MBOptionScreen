extern alias v4;

using HarmonyLib.BUTR.Extensions;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionDropdownWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionDropdown
    {
        /// <inheritdoc/>
        public int SelectedIndex { get; }

        public MCMv3PropertyDefinitionDropdownWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            SelectedIndex = AccessTools2.Property(type, nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
        }
    }
}