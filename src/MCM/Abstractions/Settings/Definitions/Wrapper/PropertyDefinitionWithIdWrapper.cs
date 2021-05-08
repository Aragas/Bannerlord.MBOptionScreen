﻿using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class PropertyDefinitionWithIdWrapper : BasePropertyDefinitionWrapper, IPropertyDefinitionWithId
    {
        /// <inheritdoc/>
        public string Id { get; }

        public PropertyDefinitionWithIdWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            Id = AccessTools2.Property(type, nameof(Id))?.GetValue(@object) as string ?? string.Empty;
        }
    }
}