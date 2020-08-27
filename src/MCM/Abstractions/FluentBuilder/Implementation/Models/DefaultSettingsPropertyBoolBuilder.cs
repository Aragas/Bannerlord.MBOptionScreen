using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation.Models
{
    public class DefaultSettingsPropertyBoolBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyBoolBuilder>,
        ISettingsPropertyBoolBuilder,
        IPropertyDefinitionBool
    {
        internal DefaultSettingsPropertyBoolBuilder(string id, string name, IRef @ref)
            : base(id, name, @ref)
        {
            SettingsPropertyBuilder = this;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionBoolWrapper(this),
            new PropertyDefinitionWithIdWrapper(this),
        };
    }
}