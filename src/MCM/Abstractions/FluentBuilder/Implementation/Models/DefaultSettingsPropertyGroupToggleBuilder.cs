using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation.Models
{
    public class DefaultSettingsPropertyGroupToggleBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyGroupToggleBuilder>,
        ISettingsPropertyGroupToggleBuilder,
        IPropertyDefinitionGroupToggle
    {
        /// <inheritdoc/>
        public bool IsToggle { get; } = true;
        
        internal DefaultSettingsPropertyGroupToggleBuilder(string id, string name, IRef @ref)
            : base(id, name, @ref)
        {
            SettingsPropertyBuilder = this;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionGroupToggleWrapper(this),
            new PropertyDefinitionWithIdWrapper(this),
        };
    }
}