﻿using TaleWorlds.Localization;

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

            DisplayName = new TextObject(type.GetProperty(nameof(DisplayName))?.GetValue(@object) as string ?? "ERROR").ToString();
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? -1;
            RequireRestart = type.GetProperty(nameof(RequireRestart))?.GetValue(@object) as bool? ?? true;
            HintText = new TextObject(type.GetProperty(nameof(HintText))?.GetValue(@object) as string ?? "ERROR").ToString();
        }
    }
}