﻿extern alias v4;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Adapter.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionWithEditableMinMaxWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionWithEditableMinMax
    {
        /// <inheritdoc/>
        public decimal EditableMinValue { get; }
        /// <inheritdoc/>
        public decimal EditableMaxValue { get; }

        public MCMv3PropertyDefinitionWithEditableMinMaxWrapper(object @object) : base(@object)
        {
            EditableMinValue = @object.GetType().GetProperty(nameof(EditableMinValue))?.GetValue(@object) as decimal? ?? 0;
            EditableMaxValue = @object.GetType().GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as decimal? ?? 0;
        }
    }
}