using MCM.Abstractions;
using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System.Collections.Generic;

namespace MCM.Implementation.FluentBuilder.Models
{
    internal sealed class DefaultSettingsPropertyBoolBuilder :
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