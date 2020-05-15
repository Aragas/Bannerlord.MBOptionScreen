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
        internal DefaultSettingsPropertyBoolBuilder(string name, IRef @ref)
            : base(name, @ref)
        {
            SettingsPropertyBuilder = this;
        }

        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionBoolWrapper(this),
        };
    }
}