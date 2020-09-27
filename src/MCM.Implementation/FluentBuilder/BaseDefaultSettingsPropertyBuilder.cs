using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Implementation.FluentBuilder
{
    internal abstract class BaseDefaultSettingsPropertyBuilder<TSettingsPropertyBuilder> :
        ISettingsPropertyBuilder<TSettingsPropertyBuilder>,
        IPropertyDefinitionBase,
        IPropertyDefinitionWithId
        where TSettingsPropertyBuilder : ISettingsPropertyBuilder
    {
        protected TSettingsPropertyBuilder SettingsPropertyBuilder { get; set; } = default!;

        /// <inheritdoc/>
        public string Name { get; }
        /// <inheritdoc/>
        public string Id { get; }
        /// <inheritdoc/>
        public IRef PropertyReference { get; }

        /// <inheritdoc/>
        public string DisplayName => Name;
        /// <inheritdoc/>
        public int Order { get; private set; }
        /// <inheritdoc/>
        public bool RequireRestart { get; private set; }
        /// <inheritdoc/>
        public string HintText { get; private set; } = string.Empty;

        protected BaseDefaultSettingsPropertyBuilder(string id, string name, IRef @ref)
        {
            Id = id;
            Name = name;
            PropertyReference = @ref;
        }

        /// <inheritdoc/>
        public TSettingsPropertyBuilder SetOrder(int value) { Order = value; return SettingsPropertyBuilder; }
        /// <inheritdoc/>
        public TSettingsPropertyBuilder SetRequireRestart(bool value) { RequireRestart = value; return SettingsPropertyBuilder; }
        /// <inheritdoc/>
        public TSettingsPropertyBuilder SetHintText(string value) { HintText = value; return SettingsPropertyBuilder; }

        /// <inheritdoc/>
        public abstract IEnumerable<IPropertyDefinitionBase> GetDefinitions();
    }
}