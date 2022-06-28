
using MCM.Common;

namespace MCM.Abstractions.Wrapper
{
    public abstract class BasePropertyDefinitionWrapper : IPropertyDefinitionBase
    {
        /// <inheritdoc/>
        public string DisplayName { get; }
        /// <inheritdoc/>
        public int Order { get; }
        /// <inheritdoc/>
        public bool RequireRestart { get; }
        /// <inheritdoc/>
        public string HintText { get; }

        protected BasePropertyDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            DisplayName = LocalizationUtils.Localize(type.GetProperty(nameof(DisplayName))?.GetValue(@object) as string ?? "ERROR");
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? -1;
            RequireRestart = type.GetProperty(nameof(RequireRestart))?.GetValue(@object) as bool? ?? true;
            HintText = LocalizationUtils.Localize(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR");
        }
    }
}