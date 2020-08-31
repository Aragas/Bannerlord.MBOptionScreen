extern alias v4;

using HarmonyLib;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionDropdownWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionDropdown
    {
        /// <inheritdoc/>
        public int SelectedIndex { get; }

        public MCMv3PropertyDefinitionDropdownWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            SelectedIndex = AccessTools.Property(type, nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
        }
    }
}