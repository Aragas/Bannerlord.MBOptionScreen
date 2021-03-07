using Bannerlord.BUTR.Shared.Helpers;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
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

            DisplayName = TextObjectHelper.Create(type.GetProperty(nameof(DisplayName))?.GetValue(@object) as string ?? "ERROR")?.ToString() ?? "ERROR";;
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? -1;
            RequireRestart = type.GetProperty(nameof(RequireRestart))?.GetValue(@object) as bool? ?? true;
            HintText = TextObjectHelper.Create(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR")?.ToString() ?? "ERROR";;
        }
    }
}